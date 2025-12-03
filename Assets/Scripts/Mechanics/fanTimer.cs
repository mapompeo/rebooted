using System.Collections;
using UnityEngine;

public class FanTimer : MonoBehaviour
{
    [Header("Modo de Operação")]
    [SerializeField] private bool sempreLigado = false; 

    [Header("Configurações de Ritmo")]
    [SerializeField] private float tempoTroca = 2f;    
    [SerializeField] private float tempoAntecipacao = 0.5f; 
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

        // procura o objeto filho 'wind' automaticamente
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
            // se for sempre ligado, define maximo e toca animacao diretamente
            SetAlpha(opacidadeMaxima);
            _windCollider.enabled = true;
            
            if(_animator != null) 
            {
                _animator.Play("fanSpin"); 
                _animator.SetBool("Ligado", true);
            }
        }
        else
        {
            // senao, inicia o ciclo do temporizador
            StartCoroutine(CicloVento());
        }
    }

    private void Update()
    {
        if (moverFan) MoverObjeto();
    }

    private void MoverObjeto()
    {
        // movimento de onda (vai e volta) usando seno
        float oscilacao = Mathf.Sin(Time.time * velocidadeMovimento);
        float novoX = _posicaoInicial.x + (oscilacao * distanciaX);
        float novoY = _posicaoInicial.y + (oscilacao * distanciaY);
        transform.position = new Vector3(novoX, novoY, _posicaoInicial.z);
    }

    private IEnumerator CicloVento()
    {
        bool estadoAtual = comecaLigado;

        // configuracao inicial (sem animacao de fade)
        if (estadoAtual)
        {
            SetAlpha(opacidadeMaxima);
            _windCollider.enabled = true;
            
            if(_animator != null) _animator.Play("fanSpin"); 
            if(_animator != null) _animator.SetBool("Ligado", true);
        }
        else
        {
            SetAlpha(opacidadeMinima);
            _windCollider.enabled = false;
            
            if(_animator != null) _animator.Play("fanIdle"); 
            if(_animator != null) _animator.SetBool("Ligado", false);
        }

        while (true)
        {
            // espera o tempo definido para o estado atual
            yield return new WaitForSeconds(tempoTroca);

            estadoAtual = !estadoAtual;

            if (estadoAtual) 
            {
                // === hora de ligar ===
                // 1. notifica o animator
                if(_animator != null) _animator.SetBool("Ligado", true);

                // 2. espera um tempo para a animacao iniciar
                yield return new WaitForSeconds(tempoAntecipacao);

                // 3. ativa a fisica e inicia o fade in
                _windCollider.enabled = true; 
                yield return StartCoroutine(FadeRoutine(opacidadeMinima, opacidadeMaxima));
            }
            else 
            {
                // === hora de desligar ===
                // 1. notifica o animator
                if(_animator != null) _animator.SetBool("Ligado", false);

                // 2. espera um tempo
                yield return new WaitForSeconds(tempoAntecipacao);

                // 3. desativa a fisica e inicia o fade out
                _windCollider.enabled = false; 
                yield return StartCoroutine(FadeRoutine(opacidadeMaxima, opacidadeMinima));
            }
        }
    }

    // realiza a transicao suave de opacidade
    private IEnumerator FadeRoutine(float alphaInicial, float alphaFinal)
    {
        float timer = 0f;
        while (timer < tempoFade)
        {
            timer += Time.deltaTime;
            // lerp calcula a transicao suave de a para b
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