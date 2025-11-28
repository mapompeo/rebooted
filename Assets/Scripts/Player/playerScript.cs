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
        _rb2D.linearVelocity = new Vector2(moveInput * moveSpeed, _rb2D.linearVelocity.y);

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