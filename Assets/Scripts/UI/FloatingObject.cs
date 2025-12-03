using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    // cria a lista de opcoes pro inspector
    public enum TipoMovimento { Flutuando, Quicando, Parado }

    [Header("Configurações")]
    public TipoMovimento tipo = TipoMovimento.Flutuando; // a opcao escolhida
    
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

        // verifica qual movimento foi escolhido
        switch (tipo)
        {
            case TipoMovimento.Parado:
                // nao faz nada, fica imovel
                break;

            case TipoMovimento.Flutuando:
                // usa seno pra fazer uma onda suave que sobe e desce (-1 a 1)
                novoY += Mathf.Sin(Time.time * velocidade) * distancia;
                break;

            case TipoMovimento.Quicando:
                // usa seno absoluto (sem numeros negativos) pra parecer um quique (0 a 1)
                novoY += Mathf.Abs(Mathf.Sin(Time.time * velocidade)) * distancia;
                break;
        }

        // aplica a nova posicao calculada
        transform.position = new Vector3(transform.position.x, novoY, transform.position.z);
    }
}