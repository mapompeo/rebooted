using UnityEngine;

public class RamProjectile : MonoBehaviour
{
    // Cria um menu de opções no Inspector
    public enum Direcao { Esquerda, Direita, Baixo, Cima }

    [Header("Movimento")]
    [SerializeField] private float velocidade = 5f;
    [SerializeField] private Direcao direcaoMovimento = Direcao.Esquerda; // Escolha a direção aqui

    [Header("Limites (Onde ele volta)")]
    [Tooltip("A coordenada X ou Y onde ele 'morre'. Ex: Se for pra Direita, coloque um valor X positivo alto.")]
    [SerializeField] private float limiteParaResetar = -10f;

    [Header("Aleatoriedade (Ao Resetar)")]
    [Tooltip("Variação da posição no eixo contrário (Ex: Se anda na horizontal, varia a altura Y)")]
    [SerializeField] private float variacaoMin = -2f;
    [SerializeField] private float variacaoMax = 2f;

    private Vector3 _posicaoInicial;

    private void Start()
    {
        _posicaoInicial = transform.position;
    }

    private void Update()
    {
        Mover();
        ChecarLimite();
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

        transform.Translate(vetorDirecao * velocidade * Time.deltaTime);
    }

    private void ChecarLimite()
    {
        bool passouDoLimite = false;

        switch (direcaoMovimento)
        {
            // Se vai pra Esquerda/Baixo, reseta se ficar MENOR que o limite
            case Direcao.Esquerda:
                passouDoLimite = transform.position.x < limiteParaResetar;
                break;
            case Direcao.Baixo:
                passouDoLimite = transform.position.y < limiteParaResetar;
                break;

            // Se vai pra Direita/Cima, reseta se ficar MAIOR que o limite
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
        float aleatorio = Random.Range(variacaoMin, variacaoMax);

        // Se o movimento é Horizontal, a gente varia a Altura (Y)
        if (direcaoMovimento == Direcao.Esquerda || direcaoMovimento == Direcao.Direita)
        {
            novaPosicao.y += aleatorio;
        }
        // Se o movimento é Vertical, a gente varia a Lateral (X)
        else
        {
            novaPosicao.x += aleatorio;
        }

        transform.position = novaPosicao;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 centro = Application.isPlaying ? _posicaoInicial : transform.position;

        // Desenha a linha de limite dependendo da direção escolhida
        if (direcaoMovimento == Direcao.Esquerda || direcaoMovimento == Direcao.Direita)
        {
            // Limite Vertical (Parede)
            Gizmos.DrawLine(new Vector3(limiteParaResetar, -100, 0), new Vector3(limiteParaResetar, 100, 0));
            
            // Onde nasce (Verde)
            Gizmos.color = Color.green;
            Gizmos.DrawLine(centro + Vector3.up * variacaoMin, centro + Vector3.up * variacaoMax);
        }
        else
        {
            // Limite Horizontal (Chão/Teto)
            Gizmos.DrawLine(new Vector3(-100, limiteParaResetar, 0), new Vector3(100, limiteParaResetar, 0));
            
            // Onde nasce (Verde)
            Gizmos.color = Color.green;
            Gizmos.DrawLine(centro + Vector3.right * variacaoMin, centro + Vector3.right * variacaoMax);
        }
    }
}