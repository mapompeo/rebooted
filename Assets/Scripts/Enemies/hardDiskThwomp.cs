using UnityEngine;

public class HardDiskThwomp : MonoBehaviour
{
    [Header("Configurações Gerais")]
    [SerializeField] private float velocidadeQueda = 15f;
    [SerializeField] private float velocidadeSubida = 3f;
    [SerializeField] private LayerMask camadaChao; // O que é considerado "Chão" para ele parar
    [SerializeField] private Transform checadorDeChao; // Um objeto vazio na parte de baixo do HD

    [Header("Modo de Operação")]
    [SerializeField] private bool sensivelAoPlayer = true; // TRUE = Cai quando vê player. FALSE = Cai por tempo.
    
    [Header("Configuração - Se for Sensível ao Player")]
    [SerializeField] private LayerMask camadaPlayer; // Para não detectar inimigos ou chão
    [SerializeField] private float alturaDeteccao = 10f; // Comprimento do "laser"
    [SerializeField] private float larguraDeteccao = 1f; // Largura da área de detecção

    [Header("Configuração - Se for por Tempo")]
    [SerializeField] private float intervaloQueda = 2f; // Tempo de espera lá em cima
    [SerializeField] private bool comecaCaindo = false; // Se deve cair assim que o jogo abre

    // Estados da Máquina
    private enum Estado { Esperando, Caindo, Subindo }
    private Estado _estadoAtual;

    private Vector3 _posicaoInicial;
    private float _timer;

    private void Start()
    {
        _posicaoInicial = transform.position;
        _estadoAtual = Estado.Esperando;

        // Se for por tempo e começar caindo, zera o timer. Senão, inicia o timer.
        if (!sensivelAoPlayer && comecaCaindo) _timer = intervaloQueda;
        else _timer = 0f;
    }

    private void Update()
    {
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
        // Garante que fique travado na posição inicial
        transform.position = _posicaoInicial;

        if (sensivelAoPlayer)
        {
            // --- MODO THWOMP (Detector) ---
            // Lança uma caixa invisível para baixo para procurar o player
            RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(larguraDeteccao, 0.1f), 0, Vector2.down, alturaDeteccao, camadaPlayer);
            
            if (hit.collider != null)
            {
                _estadoAtual = Estado.Caindo;
            }
        }
        else
        {
            // --- MODO TEMPORIZADOR ---
            _timer += Time.deltaTime;
            if (_timer >= intervaloQueda)
            {
                _estadoAtual = Estado.Caindo;
                _timer = 0; // Reseta para a próxima
            }
        }
    }

    private void ComportamentoCaindo()
    {
        // Move para baixo rápido
        transform.Translate(Vector3.down * velocidadeQueda * Time.deltaTime);

        // Verifica se bateu no chão
        // Usamos um OverlapCircle pequeno no pé do objeto
        bool tocouChao = Physics2D.OverlapCircle(checadorDeChao.position, 0.1f, camadaChao);

        if (tocouChao)
        {
            // Poderia adicionar um som de impacto aqui ou tremer a câmera
            _estadoAtual = Estado.Subindo;
        }
    }

    private void ComportamentoSubindo()
    {
        // Move em direção à posição inicial
        transform.position = Vector3.MoveTowards(transform.position, _posicaoInicial, velocidadeSubida * Time.deltaTime);

        // Se chegou no topo (distância quase zero)
        if (Vector3.Distance(transform.position, _posicaoInicial) < 0.01f)
        {
            _estadoAtual = Estado.Esperando;
            _timer = 0f; // Reinicia a contagem do tempo (só conta a partir de agora)
        }
    }

    // Desenha os Gizmos para você ver a área de detecção no Editor
    private void OnDrawGizmos()
    {
        if (sensivelAoPlayer)
        {
            Gizmos.color = Color.red;
            // Desenha o raio de visão
            Gizmos.DrawWireCube(transform.position + Vector3.down * (alturaDeteccao / 2), new Vector3(larguraDeteccao, alturaDeteccao, 0));
        }

        if (checadorDeChao != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(checadorDeChao.position, 0.1f);
        }
    }
}