using UnityEngine;

public class CapacitorWalk : MonoBehaviour
{
    // cria esse enum pra facilitar a escolha no inspector
    public enum Direcao { Esquerda = -1, Direita = 1 }

    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidade = 3f;
    [SerializeField] private Direcao direcaoInicial = Direcao.Esquerda; 
    
    [Header("Comportamento")]
    // se marcar isso aqui, o inimigo nao liga de cair no void
    [SerializeField] private bool podeCair = true; 

    [Header("Sensores")]
    [SerializeField] private LayerMask layerParede; 
    [SerializeField] private Transform verificadorParede; 

    private Rigidbody2D _rb;
    private int _direcao; 

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        // pega o valor int (-1 ou 1) do enum escolhido no inspector
        _direcao = (int)direcaoInicial;

        // se escolher comecar pra direita, vira o sprite para nao andar de costas
        if (_direcao == 1)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * -1; 
            transform.localScale = escala;
        }
        else
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x); 
            transform.localScale = escala;
        }
    }

    private void FixedUpdate()
    {
        // movimenta o objeto mexendo na fisica
        _rb.linearVelocity = new Vector2(velocidade * _direcao, _rb.linearVelocity.y);
    }

    private void Update()
    {
        // solta um raio invisivel pra frente pra detectar colisoes
        RaycastHit2D hitFrente = Physics2D.Raycast(verificadorParede.position, Vector2.right * _direcao, 0.1f);

        if (hitFrente.collider != null)
        {
            // ignora se o raio bater no proprio colisor
            if (hitFrente.collider.gameObject == gameObject) return;

            // calculo bitwise pra verificar se o objeto esta na layer de parede
            bool bateuNaParede = (layerParede.value & (1 << hitFrente.collider.gameObject.layer)) > 0;
            // ou se bateu em algo que mata
            bool bateuNoPerigo = hitFrente.collider.CompareTag("Kill");

            if (bateuNaParede || bateuNoPerigo)
            {
                Virar();
                return; // se ja virou, nao precisa checar buraco
            }
        }

        // se nao puder cair, checa o chao
        if (!podeCair)
        {
            // raio pra baixo a partir da frente do objeto
            RaycastHit2D hitChao = Physics2D.Raycast(verificadorParede.position, Vector2.down, 0.5f, layerParede);

            // se nao bateu em nada (null), e buraco, entao volta
            if (hitChao.collider == null)
            {
                Virar();
            }
        }
    }

    private void Virar()
    {
        _direcao *= -1; // inverte o valor matematico (-1 vira 1 e vice versa)
        
        // espelha o sprite visualmente
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
    
    private void OnDrawGizmos()
    {
        if (verificadorParede != null)
        {
            // desenha as linhas no editor pra ajudar no ajuste
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(verificadorParede.position, verificadorParede.position + Vector3.right * _direcao * 0.1f);

            if (!podeCair)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(verificadorParede.position, verificadorParede.position + Vector3.down * 0.5f);
            }
        }
    }
}