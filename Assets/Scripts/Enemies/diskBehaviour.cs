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

    // Variável interna para lembrar onde ele começou
    private Vector3 _posicaoInicial;

    private void Start()
    {
        // Guarda a posição inicial como ponto central (âncora)
        _posicaoInicial = transform.position;
    }

    private void Update()
    {
        MoverObjeto();
    }

    private void MoverObjeto()
    {
        // Mathf.Sin cria uma onda que vai de -1 a 1 suavemente baseado no tempo
        // Multiplicando pelo tempo, criamos o ritmo de vai-e-vem
        float oscilacao = Mathf.Sin(Time.time * velocidadeMovimento);

        // Calcula a nova posição somando o deslocamento à posição original
        float novoX = _posicaoInicial.x + (oscilacao * distanciaX);
        float novoY = _posicaoInicial.y + (oscilacao * distanciaY);

        // Aplica ao objeto
        transform.position = new Vector3(novoX, novoY, _posicaoInicial.z);
    }
}