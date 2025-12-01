using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    // Cria a lista de opções para aparecer no Inspector
    public enum TipoMovimento { Flutuando, Quicando, Parado }

    [Header("Configurações")]
    public TipoMovimento tipo = TipoMovimento.Flutuando; // A opção escolhida
    
    public float velocidade = 2f;
    public float distancia = 0.5f;
    
    private Vector3 _posInicial;

    void Start()
    {
        _posInicial = transform.position;
    }

    void Update()
    {
        float novoY = _posInicial.y;

        // Verifica qual opção você escolheu na lista
        switch (tipo)
        {
            case TipoMovimento.Parado:
                // Não faz nada, mantém a posição original
                break;

            case TipoMovimento.Flutuando:
                // Onda Suave (-1 até 1): Sobe e desce passando pela origem
                novoY += Mathf.Sin(Time.time * velocidade) * distancia;
                break;

            case TipoMovimento.Quicando:
                // Onda de Pulo (0 até 1): O Mathf.Abs transforma números negativos em positivos.
                // Visualmente, parece que a seta bate no chão e sobe de novo.
                novoY += Mathf.Abs(Mathf.Sin(Time.time * velocidade)) * distancia;
                break;
        }

        // Aplica a nova altura
        transform.position = new Vector3(transform.position.x, novoY, transform.position.z);
    }
}