using UnityEngine;

public class DiskMovement : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("Quão rápido ele vai e volta")]
    [SerializeField] private float velocidadeMovimento = 2f; 

    [Tooltip("Distância total que ele anda para os lados")]
    [SerializeField] private float distanciaX = 3f;    

    [Tooltip("Distância total que ele anda para cima/baixo")]
    [SerializeField] private float distanciaY = 0f;    

    private Vector3 _posicaoInicial;

    private void Start()
    {
        // salva a posicao inicial pra usar de ancora pro movimento
        _posicaoInicial = transform.position;
    }

    private void Update()
    {
        MoverObjeto();
    }

    private void MoverObjeto()
    {
        // mathf.sin cria uma onda suave de -1 a 1 baseada no tempo
        float oscilacao = Mathf.Sin(Time.time * velocidadeMovimento);

        // calcula a nova posicao baseada na oscilacao
        float novoX = _posicaoInicial.x + (oscilacao * distanciaX);
        float novoY = _posicaoInicial.y + (oscilacao * distanciaY);

        // aplica a nova posicao ao objeto
        transform.position = new Vector3(novoX, novoY, _posicaoInicial.z);
    }
}