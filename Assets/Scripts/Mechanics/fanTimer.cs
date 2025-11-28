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
                // CORRIGIDO: Nome exato "fanSpin"
                _animator.Play("fanSpin"); 
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
            
            // CORRIGIDO: Nome exato "fanSpin"
            if(_animator != null) _animator.Play("fanSpin"); 
            if(_animator != null) _animator.SetBool("Ligado", true);
        }
        else
        {
            SetAlpha(opacidadeMinima);
            _windCollider.enabled = false;
            
            // CORRIGIDO: Nome exato "fanIdle"
            if(_animator != null) _animator.Play("fanIdle"); 
            if(_animator != null) _animator.SetBool("Ligado", false);
        }

        while (true)
        {
            // Espera o tempo do ciclo ATUAL
            yield return new WaitForSeconds(tempoTroca);

            // Inverte o estado
            estadoAtual = !estadoAtual;

            if (estadoAtual) 
            {
                // === VAI LIGAR ===
                if(_animator != null) _animator.SetBool("Ligado", true);

                // Espera a animação pegar embalo
                yield return new WaitForSeconds(tempoAntecipacao);

                _windCollider.enabled = true; 
                yield return StartCoroutine(FadeRoutine(opacidadeMinima, opacidadeMaxima));
            }
            else 
            {
                // === VAI DESLIGAR ===
                if(_animator != null) _animator.SetBool("Ligado", false);

                // Espera um pouco antes de sumir o vento
                yield return new WaitForSeconds(tempoAntecipacao);

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