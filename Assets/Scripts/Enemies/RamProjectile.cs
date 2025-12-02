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
    [Tooltip("A coordenada X ou Y onde ele 'morre'. Ex: Se for pra Direita, coloque um valor X positivo alto.")]
    [SerializeField] private float limiteParaResetar = -10f;

    [Header("Aleatoriedade (Ao Resetar)")]
    [Tooltip("Variação da posição no eixo contrário (Ex: Se anda na horizontal, varia a altura Y)")]
    [SerializeField] private float variacaoMin = -2f;
    [SerializeField] private float variacaoMax = 2f;

    private Vector3 _posicaoInicial;
    private float _velocidadeAtual; // Variável interna para guardar a velocidade sorteada

    private void Start()
    {
        _posicaoInicial = transform.position;
        // Já começa com uma velocidade aleatória
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

        switch (direcaoMovimento)
        {
            case Direcao.Esquerda: vetorDirecao = Vector3.left; break;
            case Direcao.Direita:  vetorDirecao = Vector3.right; break;
            case Direcao.Baixo:    vetorDirecao = Vector3.down; break;
            case Direcao.Cima:     vetorDirecao = Vector3.up; break;
        }

        // Usa a velocidade sorteada
        transform.Translate(vetorDirecao * _velocidadeAtual * Time.deltaTime);
    }

    private void ChecarLimite()
    {
        bool passouDoLimite = false;

        switch (direcaoMovimento)
        {
            case Direcao.Esquerda:
                passouDoLimite = transform.position.x < limiteParaResetar;
                break;
            case Direcao.Baixo:
                passouDoLimite = transform.position.y < limiteParaResetar;
                break;
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

        // Variação de Posição
        if (direcaoMovimento == Direcao.Esquerda || direcaoMovimento == Direcao.Direita)
        {
            novaPosicao.y += aleatorioPosicao;
        }
        else
        {
            novaPosicao.x += aleatorioPosicao;
        }

        transform.position = novaPosicao;
        
        // Variação de Velocidade (Sorteia de novo para a próxima passagem)
        SortearVelocidade();
    }

    private void OnDrawGizmos()
    {
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