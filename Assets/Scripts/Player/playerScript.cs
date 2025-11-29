using UnityEngine;
using UnityEngine.SceneManagement; // 1. OBRIGATÓRIO pra mudar de cena

public class playerScript : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float moveSpeed = 5f;
    
    
    [Header("Verificação de Chão")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("Efeitos Visuais")]
    [SerializeField] private FlashEffect flashEffectScript; 

    private Rigidbody2D _rb2D;
    private SpriteRenderer _spriteRenderer;
    private bool _isGrounded;
    private Vector3 _posicaoInicial; 

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _posicaoInicial = transform.position;
    }

    private void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        Movimentation();
    }

    private void Movimentation()
{
    float moveInput = Input.GetAxisRaw("Horizontal");
    float velocidadeAtualX = _rb2D.linearVelocity.x;

    
    bool estaNoEmbalo = Mathf.Abs(velocidadeAtualX) > moveSpeed;

    if (estaNoEmbalo)
    {
        
        // Pergunta: Estou tentando ir para o MESMO lado que o vento me joga?
        // (Mathf.Sign compara se os sinais são iguais, tipo + com +)
        bool mesmaDirecao = Mathf.Sign(moveInput) == Mathf.Sign(velocidadeAtualX);

        if (moveInput != 0 && mesmaDirecao)
        {
             // Se eu aperto pra ir a favor do vento, NÃO FAZ NADA.
             // Deixa o vento me levar (senão eu limitaria minha velocidade pra baixo).
        }
        else if (moveInput != 0 && !mesmaDirecao)
        {
             // Se eu aperto CONTRA o vento (Frear):
             // AQUI ESTÁ A CORREÇÃO: Não defina a velocidade. Apenas empurre contra.
             // "airBrakeForce" é uma variavel nova que vamos criar (uns 30 ou 40)
             _rb2D.AddForce(new Vector2(moveInput * 1f, 0)); 
        }
        
        // Se moveInput for 0, não faz nada, deixa o atrito natural agir.
    }
    else
    {
        // === ZONA DE CONTROLE NORMAL (CHÃO/DEVAGAR) ===
        // Aqui sim nós somos "Autoritários" e definimos a velocidade exata.
        
        if (moveInput != 0)
        {
            _rb2D.linearVelocity = new Vector2(moveInput * moveSpeed, _rb2D.linearVelocity.y);
        }
        else
        {
            // Freio de chão instantâneo
            _rb2D.linearVelocity = new Vector2(0, _rb2D.linearVelocity.y);
        }
    }

    // --- VISUAL E PULO (MANTÉM IGUAL) ---
    if (moveInput > 0) _spriteRenderer.flipX = false;
    else if (moveInput < 0) _spriteRenderer.flipX = true;

    if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && _isGrounded)
    {
        _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x, jumpSpeed);
    }
}
    
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red; 
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Lógica de Morte (Volta pro início)
        if (collision.CompareTag("Kill"))
        {
            if (flashEffectScript != null)
            {
                flashEffectScript.TriggerFlash();
            }
            transform.position = _posicaoInicial;
            _rb2D.linearVelocity = Vector2.zero; 
        }
        // 2. Lógica de Próxima Fase (Vai pra frente)
        else if (collision.CompareTag("NextLevel"))
        {
            // Pega o número da cena atual e soma 1.
            // Ex: Se está na cena 0, carrega a cena 1.
            int proximaCena = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(proximaCena);
        }
    }
}