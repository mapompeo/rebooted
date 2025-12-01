using UnityEngine;
using UnityEngine.SceneManagement;

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
    private Animator _animator; // 1. NOVA VARIÁVEL
    
    private bool _isGrounded;
    private Vector3 _posicaoInicial; 

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>(); // 2. PEGAR O COMPONENTE
        
        _posicaoInicial = transform.position;
    }

    private void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        Movimentation();
        UpdateAnimations(); // 3. CHAMAR A FUNÇÃO DE ANIMAÇÃO
    }

    private void Movimentation()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float velocidadeAtualX = _rb2D.linearVelocity.x;

        // Verifica se está mais rápido que o normal (Vento ou Imprecisão matemática)
        bool estaNoEmbalo = Mathf.Abs(velocidadeAtualX) > moveSpeed;

        if (estaNoEmbalo)
        {
            // === ZONA DE ALTA VELOCIDADE ===
            
            bool mesmaDirecao = Mathf.Sign(moveInput) == Mathf.Sign(velocidadeAtualX);

            if (moveInput != 0 && mesmaDirecao)
            {
                 // A favor do movimento: não atrapalha, deixa fluir
            }
            else if (moveInput != 0 && !mesmaDirecao)
            {
                 // Contra o movimento: Freio de emergência (AddForce)
                 _rb2D.AddForce(new Vector2(moveInput * 30f, 0)); // Aumentei pra 30f pra ficar responsivo
            }
            // --- NOVO: SE SOLTAR A TECLA, APLICA ATRITO ---
            else if (moveInput == 0)
            {
                // Multiplica a velocidade atual por 0.95 a cada frame.
                // Isso cria um desaceleramento suave (drag) sem travar a gravidade.
                // Quanto menor o numero (ex: 0.90), mais rapido ele para.
                _rb2D.linearVelocity = new Vector2(velocidadeAtualX * 0.95f, _rb2D.linearVelocity.y);
            }
        }
        else
        {
            // === ZONA DE CONTROLE NORMAL (CHÃO/DEVAGAR) ===
            if (moveInput != 0)
            {
                _rb2D.linearVelocity = new Vector2(moveInput * moveSpeed, _rb2D.linearVelocity.y);
            }
            else
            {
                // Freio seco quando está numa velocidade normal
                _rb2D.linearVelocity = new Vector2(0, _rb2D.linearVelocity.y);
            }
        }

        // --- VISUAL (VIRAR O SPRITE) ---
        if (moveInput > 0) _spriteRenderer.flipX = false;
        else if (moveInput < 0) _spriteRenderer.flipX = true;

        // --- PULO ---
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && _isGrounded)
        {
            _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x, jumpSpeed);
        }
    }

    // 4. NOVA FUNÇÃO QUE CONTROLA O ANIMATOR
    private void UpdateAnimations()
    {
        if (_animator != null)
        {
            // Envia a velocidade sempre positiva para o parâmetro "Speed"
            _animator.SetFloat("Speed", Mathf.Abs(_rb2D.linearVelocity.x));
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
        if (collision.CompareTag("Kill"))
        {
            if (flashEffectScript != null)
            {
                flashEffectScript.TriggerFlash();
            }
            transform.position = _posicaoInicial;
            _rb2D.linearVelocity = Vector2.zero; 
        }
        else if (collision.CompareTag("NextLevel"))
        {
            int proximaCena = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(proximaCena);
        }
        else if (collision.CompareTag("BacktoMenu"))
        {
            SceneManager.LoadScene("menu");
        }
        
        else if (collision.CompareTag("MemoryAdress"))
        {
            Destroy(collision.gameObject);
        }
    }
}