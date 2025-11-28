using System.Collections;
using UnityEngine;

public class FanTimer : MonoBehaviour
{
    [Header("Modo de Operação")]
    [SerializeField] private bool sempreLigado = false; 

    [Header("Configurações de Ritmo")]
    [SerializeField] private float tempoTroca = 2f;    // Tempo que fica ligado/desligado de fato
    [SerializeField] private float tempoAntecipacao = 0.5f; // Tempo de aviso (animação antes do vento)
    [SerializeField] private float tempoFade = 0.5f;   
    [SerializeField] private bool comecaLigado = true; 

    [Header("Configurações Visuais")]
    [Range(0f, 1f)] [SerializeField] private float opacidadeMinima = 0f; 
    [Range(0f, 1f)] [SerializeField] private float opacidadeMaxima = 1f; 

    [Header("Movimento")]
    [SerializeField] private bool moverFan = false;    
    [SerializeField] private float velocidadeMovimento = 2f; 
    [SerializeField] private float distanciaX = 3f;    
    [SerializeField] private float distanciaY = 0f;    

    private SpriteRenderer _windSprite;
    private Collider2D _windCollider;
    private Vector3 _posicaoInicial; 
    private Animator _animator;

    private void Start()
    {
        _posicaoInicial = transform.position;
        _animator = GetComponent<Animator>();

        Transform windChild = transform.Find("Wind");
        if (windChild != null)
        {
            _windSprite = windChild.GetComponent<SpriteRenderer>();
            _windCollider = windChild.GetComponent<Collider2D>();
        }
        else
        {
            Debug.LogError("ERRO: Objeto 'Wind' não encontrado.");
            return;
        }

        if (sempreLigado)
        {
            SetAlpha(opacidadeMaxima);
            _windCollider.enabled = true;
            
            if(_animator != null) 
            {
                // FORÇA BRUTA: Se estiver sempre ligado, toca o Spin direto
                // ignorando transições pra não travar.
                _animator.Play("Fan_Spin"); 
                _animator.SetBool("Ligado", true);
            }
        }
        else
        {
            StartCoroutine(CicloVento());
        }
    }

    private void Update()
    {
        if (moverFan) MoverObjeto();
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

        // --- CONFIGURAÇÃO INICIAL (Sem delay) ---
        if (estadoAtual)
        {
            SetAlpha(opacidadeMaxima);
            _windCollider.enabled = true;
            if(_animator != null) _animator.Play("Fan_Spin"); // Começa girando
            if(_animator != null) _animator.SetBool("Ligado", true);
        }
        else
        {
            SetAlpha(opacidadeMinima);
            _windCollider.enabled = false;
            if(_animator != null) _animator.Play("Fan_Idle"); // Começa parado
            if(_animator != null) _animator.SetBool("Ligado", false);
        }

        while (true)
        {
            // Espera o tempo do ciclo ATUAL
            yield return new WaitForSeconds(tempoTroca);

            // Inverte o estado (decide o que vai fazer a seguir)
            estadoAtual = !estadoAtual;

            if (estadoAtual) 
            {
                // === VAI LIGAR ===
                // 1. Liga Animação (Aviso)
                if(_animator != null) _animator.SetBool("Ligado", true);

                // 2. Espera a Antecipação (0.5s)
                yield return new WaitForSeconds(tempoAntecipacao);

                // 3. Liga o Vento de fato
                _windCollider.enabled = true; 
                yield return StartCoroutine(FadeRoutine(opacidadeMinima, opacidadeMaxima));
            }
            else 
            {
                // === VAI DESLIGAR ===
                // 1. Desliga Animação (Aviso que vai parar)
                if(_animator != null) _animator.SetBool("Ligado", false);

                // 2. Espera a Antecipação (0.5s)
                yield return new WaitForSeconds(tempoAntecipacao);

                // 3. Desliga o Vento de fato
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