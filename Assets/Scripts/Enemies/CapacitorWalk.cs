using UnityEngine;

public class CapacitorWalk : MonoBehaviour
{
    // Criei esse "Tipo" novo só pra aparecer bonitinho no Inspector
    public enum Direcao { Esquerda = -1, Direita = 1 }

    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidade = 3f;
    [SerializeField] private Direcao direcaoInicial = Direcao.Esquerda; // Escolha aqui
    
    [Header("Comportamento")]
    [Tooltip("Se marcado, o inimigo cai nos buracos. Se desmarcado, ele volta.")]
    [SerializeField] private bool podeCair = true; 

    [Header("Sensores")]
    [SerializeField] private LayerMask layerParede; 
    [SerializeField] private Transform verificadorParede; 

    private Rigidbody2D _rb;
    private int _direcao; 

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        // Configura a direção inicial baseada no que você escolheu no Inspector
        _direcao = (int)direcaoInicial;

        // Se escolheu Direita, já vira o sprite no começo pra não andar de costas
        // (Assumindo que seu desenho original olha para a Esquerda)
        if (_direcao == 1)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * -1; // Força virar pra direita
            transform.localScale = escala;
        }
        else
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x); // Força virar pra esquerda (padrão)
            transform.localScale = escala;
        }
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = new Vector2(velocidade * _direcao, _rb.linearVelocity.y);
    }

    private void Update()
    {
        // 1. Detecta Obstáculos na Frente (Parede ou Kill)
        RaycastHit2D hitFrente = Physics2D.Raycast(verificadorParede.position, Vector2.right * _direcao, 0.1f);

        if (hitFrente.collider != null)
        {
            if (hitFrente.collider.gameObject == gameObject) return;

            bool bateuNaParede = (layerParede.value & (1 << hitFrente.collider.gameObject.layer)) > 0;
            bool bateuNoPerigo = hitFrente.collider.CompareTag("Kill");

            if (bateuNaParede || bateuNoPerigo)
            {
                Virar();
                return; // Se já virou por parede, não precisa checar buraco
            }
        }

        // 2. Detecta Buraco (Se NÃO puder cair)
        if (!podeCair)
        {
            // Lança um raio para BAIXO a partir do "nariz" do inimigo
            RaycastHit2D hitChao = Physics2D.Raycast(verificadorParede.position, Vector2.down, 0.5f, layerParede);

            // Se o raio NÃO bater em nada (null), significa que tem um buraco na frente
            if (hitChao.collider == null)
            {
                Virar();
            }
        }
    }

    private void Virar()
    {
        _direcao *= -1; 
        
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
    
    private void OnDrawGizmos()
    {
        if (verificadorParede != null)
        {
            // Raio da Parede (Azul)
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(verificadorParede.position, verificadorParede.position + Vector3.right * _direcao * 0.1f);

            // Raio do Buraco (Vermelho) - Só desenha se não puder cair
            if (!podeCair)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(verificadorParede.position, verificadorParede.position + Vector3.down * 0.5f);
            }
        }
    }
}