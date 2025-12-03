using UnityEngine;

public class RamProjectile : MonoBehaviour
{
    public enum Direcao { Esquerda, Direita, Baixo, Cima }

    [Header("Movimento")]
    [Tooltip("Velocidade Mínima")]
    [SerializeField] private float velocidadeMin = 4f; 
    [Tooltip("Velocidade Máxima")]
    [SerializeField] private float velocidadeMax = 8f;
    
    [SerializeField] private Direcao direcaoMovimento = Direcao.Esquerda;

    [Header("Limites (Onde ele volta)")]
    // ponto onde o objeto e resetado pro inicio
    [SerializeField] private float limiteParaResetar = -10f;

    [Header("Aleatoriedade (Ao Resetar)")]
    // varia a posicao inicial pra nao nascer sempre no mesmo lugar
    [SerializeField] private float variacaoMin = -2f;
    [SerializeField] private float variacaoMax = 2f;

    private Vector3 _posicaoInicial;
    private float _velocidadeAtual; 

    private void Start()
    {
        _posicaoInicial = transform.position;
        // sorteia uma velocidade no inicio
        SortearVelocidade();
    }

    private void Update()
    {
        Mover();
        ChecarLimite();
    }

    private void SortearVelocidade()
    {
        _velocidadeAtual = Random.Range(velocidadeMin, velocidadeMax);
    }

    private void Mover()
    {
        Vector3 vetorDirecao = Vector3.zero;

        // define a direcao do vetor baseado na escolha do inspector
        switch (direcaoMovimento)
        {
            case Direcao.Esquerda: vetorDirecao = Vector3.left; break;
            case Direcao.Direita:  vetorDirecao = Vector3.right; break;
            case Direcao.Baixo:    vetorDirecao = Vector3.down; break;
            case Direcao.Cima:     vetorDirecao = Vector3.up; break;
        }

        // move o objeto usando a velocidade sorteada
        transform.Translate(vetorDirecao * _velocidadeAtual * Time.deltaTime);
    }

    private void ChecarLimite()
    {
        bool passouDoLimite = false;

        switch (direcaoMovimento)
        {
            // se vai pra esquerda/baixo, reseta se o valor ficar menor que o limite
            case Direcao.Esquerda:
                passouDoLimite = transform.position.x < limiteParaResetar;
                break;
            case Direcao.Baixo:
                passouDoLimite = transform.position.y < limiteParaResetar;
                break;
            // se vai pra direita/cima, reseta se o valor ficar maior
            case Direcao.Direita:
                passouDoLimite = transform.position.x > limiteParaResetar;
                break;
            case Direcao.Cima:
                passouDoLimite = transform.position.y > limiteParaResetar;
                break;
        }

        if (passouDoLimite)
        {
            ResetarPosicao();
        }
    }

    private void ResetarPosicao()
    {
        Vector3 novaPosicao = _posicaoInicial;
        float aleatorioPosicao = Random.Range(variacaoMin, variacaoMax);

        // se anda de lado, varia a altura (y). se cai, varia a lateral (x)
        if (direcaoMovimento == Direcao.Esquerda || direcaoMovimento == Direcao.Direita)
        {
            novaPosicao.y += aleatorioPosicao;
        }
        else
        {
            novaPosicao.x += aleatorioPosicao;
        }

        transform.position = novaPosicao;
        
        // sorteia novamente pra proxima passagem ser diferente
        SortearVelocidade();
    }

    private void OnDrawGizmos()
    {
        // desenha as linhas no editor pra visualizar limites e spawn
        Gizmos.color = Color.red;
        Vector3 centro = Application.isPlaying ? _posicaoInicial : transform.position;

        if (direcaoMovimento == Direcao.Esquerda || direcaoMovimento == Direcao.Direita)
        {
            Gizmos.DrawLine(new Vector3(limiteParaResetar, -100, 0), new Vector3(limiteParaResetar, 100, 0));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(centro + Vector3.up * variacaoMin, centro + Vector3.up * variacaoMax);
        }
        else
        {
            Gizmos.DrawLine(new Vector3(-100, limiteParaResetar, 0), new Vector3(100, limiteParaResetar, 0));
            Gizmos.color = Color.green;
            Gizmos.DrawLine(centro + Vector3.right * variacaoMin, centro + Vector3.right * variacaoMax);
        }
    }
}