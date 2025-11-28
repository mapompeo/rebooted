using System.Collections;
using UnityEngine;

public class FanTimer : MonoBehaviour
{
    [Header("Modo de Operação")]
    [SerializeField] private bool sempreLigado = false; 

    [Header("Configurações de Ritmo")]
    [SerializeField] private float tempoTroca = 2f;    
    [SerializeField] private float tempoFade = 0.5f;   
    [SerializeField] private bool comecaLigado = true; 

    [Header("Configurações Visuais (Opacidade)")]
    [Range(0f, 1f)] [SerializeField] private float opacidadeMinima = 0f; // Vento "Desligado"
    [Range(0f, 1f)] [SerializeField] private float opacidadeMaxima = 1f; // Vento "Ligado"

    [Header("Configurações de Movimento")]
    [SerializeField] private bool moverFan = false;    
    [SerializeField] private float velocidadeMovimento = 2f; 
    [SerializeField] private float distanciaX = 3f;    
    [SerializeField] private float distanciaY = 0f;    

    // Referências internas
    private SpriteRenderer _windSprite;
    private Collider2D _windCollider;
    private Vector3 _posicaoInicial; 

    private void Start()
    {
        _posicaoInicial = transform.position;

        // --- AUTO-DETECÇÃO ---
        Transform windChild = transform.Find("Wind");

        if (windChild != null)
        {
            _windSprite = windChild.GetComponent<SpriteRenderer>();
            _windCollider = windChild.GetComponent<Collider2D>();
        }
        else
        {
            Debug.LogError("ERRO: Objeto 'Wind' não encontrado em " + name);
            return;
        }

        // --- LÓGICA DO "SEMPRE LIGADO" ---
        if (sempreLigado)
        {
            // Usa a opacidade MÁXIMA que você configurou
            SetAlpha(opacidadeMaxima);
            _windCollider.enabled = true;
        }
        else
        {
            StartCoroutine(CicloVento());
        }
    }

    private void Update()
    {
        if (moverFan)
        {
            MoverObjeto();
        }
    }

    private void MoverObjeto()
    {
        float oscilacao = Mathf.Sin(Time.time * velocidadeMovimento);
        float novoX = _posicaoInicial.x + (oscilacao * distanciaX);
        float novoY = _posicaoInicial.y + (oscilacao * distanciaY);

        transform.position = new Vector3(novoX, novoY, _posicaoInicial.z);
    }

    private IEnumerator CicloVento()
    {
        bool estadoAtual = comecaLigado;

        // Configuração inicial (Instantânea, sem fade)
        if (estadoAtual)
        {
            SetAlpha(opacidadeMaxima);
            _windCollider.enabled = true;
        }
        else
        {
            SetAlpha(opacidadeMinima);
            _windCollider.enabled = false;
        }

        while (true)
        {
            yield return new WaitForSeconds(tempoTroca);

            estadoAtual = !estadoAtual;

            if (estadoAtual)
            {
                // LIGANDO: Vai do Mínimo para o Máximo
                _windCollider.enabled = true; 
                yield return StartCoroutine(FadeRoutine(opacidadeMinima, opacidadeMaxima));
            }
            else
            {
                // DESLIGANDO: Vai do Máximo para o Mínimo
                _windCollider.enabled = false; 
                yield return StartCoroutine(FadeRoutine(opacidadeMaxima, opacidadeMinima));
            }
        }
    }

    private IEnumerator FadeRoutine(float alphaInicial, float alphaFinal)
    {
        float timer = 0f;
        while (timer < tempoFade)
        {
            timer += Time.deltaTime;
            float novoAlpha = Mathf.Lerp(alphaInicial, alphaFinal, timer / tempoFade);
            SetAlpha(novoAlpha);
            yield return null;
        }
        SetAlpha(alphaFinal);
    }

    private void SetAlpha(float alpha)
    {
        if (_windSprite != null)
        {
            Color cor = _windSprite.color;
            cor.a = alpha;
            _windSprite.color = cor;
        }
    }
}