using UnityEngine;

public class HardDiskThwomp : MonoBehaviour
{
    [Header("Configurações Gerais")]
    [SerializeField] private float velocidadeQueda = 15f;
    [SerializeField] private float velocidadeSubida = 3f;
    [SerializeField] private LayerMask camadaChao; // o que e considerado chao para parar
    [SerializeField] private Transform checadorDeChao; // objeto vazio na base

    [Header("Impacto na Câmera")]
    [SerializeField] private float duracaoShake = 0.2f; // tempo que a tela treme
    [SerializeField] private float forcaShake = 0.3f;   // forca da tremida

    [Header("Modo de Operação")]
    [SerializeField] private bool sensivelAoPlayer = true; // se true, cai quando ve o player. se false, cai por tempo
    
    [Header("Configuração - Se for Sensível ao Player")]
    [SerializeField] private LayerMask camadaPlayer; 
    [SerializeField] private float alturaDeteccao = 10f; 
    [SerializeField] private float larguraDeteccao = 1f; 

    [Header("Configuração - Se for por Tempo")]
    [SerializeField] private float intervaloQueda = 2f; 
    [SerializeField] private bool comecaCaindo = false; 

    // maquina de estados simples pra controlar o comportamento
    private enum Estado { Esperando, Caindo, Subindo }
    private Estado _estadoAtual;

    private Vector3 _posicaoInicial;
    private float _timer;
    private CameraFollow _cameraScript; // referencia pro script da camera

    private void Start()
    {
        _posicaoInicial = transform.position;
        _estadoAtual = Estado.Esperando;

        // encontra a camera principal e pega o script dela automaticamente
        if (Camera.main != null)
        {
            _cameraScript = Camera.main.GetComponent<CameraFollow>();
        }

        // configura o timer inicial dependendo do modo escolhido
        if (!sensivelAoPlayer && comecaCaindo) _timer = intervaloQueda;
        else _timer = 0f;
    }

    private void Update()
    {
        // executa a logica dependendo do estado atual
        switch (_estadoAtual)
        {
            case Estado.Esperando:
                ComportamentoEsperando();
                break;
            case Estado.Caindo:
                ComportamentoCaindo();
                break;
            case Estado.Subindo:
                ComportamentoSubindo();
                break;
        }
    }

    private void ComportamentoEsperando()
    {
        // mantem o objeto na posicao inicial
        transform.position = _posicaoInicial;

        if (sensivelAoPlayer)
        {
            // lança uma caixa invisivel pra baixo (boxcast) procurando o player
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(larguraDeteccao, 0.1f), 0, Vector2.down, alturaDeteccao, camadaPlayer);
            
            if (hit.collider != null)
            {
                _estadoAtual = Estado.Caindo;
            }
        }
        else
        {
            // conta o tempo pra cair automaticamente
            _timer += Time.deltaTime;
            if (_timer >= intervaloQueda)
            {
                _estadoAtual = Estado.Caindo;
                _timer = 0; 
            }
        }
    }

    private void ComportamentoCaindo()
    {
        // move rapidamente para baixo
        transform.Translate(Vector3.down * velocidadeQueda * Time.deltaTime);

        // verifica colisao com o chao usando um circulo na base
        bool tocouChao = Physics2D.OverlapCircle(checadorDeChao.position, 0.1f, camadaChao);

        if (tocouChao)
        {
            // chama a funcao de tremer a tela se a camera existir
            if (_cameraScript != null)
            {
                _cameraScript.TriggerShake(duracaoShake, forcaShake);
            }

            // colidiu, inicia a subida
            _estadoAtual = Estado.Subindo;
        }
    }

    private void ComportamentoSubindo()
    {
        // retorna lentamente para a posicao inicial
        transform.position = Vector3.MoveTowards(transform.position, _posicaoInicial, velocidadeSubida * Time.deltaTime);

        // se chegou no topo, volta ao estado de espera
        if (Vector3.Distance(transform.position, _posicaoInicial) < 0.01f)
        {
            _estadoAtual = Estado.Esperando;
            _timer = 0f; 
        }
    }

    // desenha as areas de deteccao para visualizacao no editor
    private void OnDrawGizmos()
    {
        if (sensivelAoPlayer)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + Vector3.down * (alturaDeteccao / 2), new Vector3(larguraDeteccao, alturaDeteccao, 0));
        }

        if (checadorDeChao != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(checadorDeChao.position, 0.1f);
        }
    }
}