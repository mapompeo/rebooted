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
    private Animator _animator; 
    
    private bool _isGrounded;
    private Vector3 _posicaoInicial; 

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>(); 
        
        _posicaoInicial = transform.position;
    }

    private void Update()
    {
        // verifica se ha chao para permitir o pulo
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        Movimentation();
        UpdateAnimations();
    }

    private void Movimentation()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        float velocidadeAtualX = _rb2D.linearVelocity.x;

        // verifica se a velocidade e maior que o normal (ex: vento)
        bool estaNoEmbalo = Mathf.Abs(velocidadeAtualX) > moveSpeed;

        if (estaNoEmbalo)
        {
            // === zona de alta velocidade ===
            
            bool mesmaDirecao = Mathf.Sign(moveInput) == Mathf.Sign(velocidadeAtualX);

            if (moveInput != 0 && mesmaDirecao)
            {
                 // se o movimento e a favor do vento, nao interfere
            }
            else if (moveInput != 0 && !mesmaDirecao)
            {
                 // se o movimento e contra, aplica forca para frear
                 _rb2D.AddForce(new Vector2(moveInput * 3f, 0));
            }
            // --- se soltou a tecla (freio automatico) ---
            else if (moveInput == 0)
            {
                // reduz a velocidade gradualmente para parar suavemente
                // (necessario pois o drag do rigidbody foi removido)
                _rb2D.linearVelocity = new Vector2(velocidadeAtualX * 0.95f, _rb2D.linearVelocity.y);
            }
        }
        else
        {
            // === zona normal (chao/baixa velocidade) ===
            // define a velocidade exata, controle total
            if (moveInput != 0)
            {
                _rb2D.linearVelocity = new Vector2(moveInput * moveSpeed, _rb2D.linearVelocity.y);
            }
            else
            {
                // parada imediata
                _rb2D.linearVelocity = new Vector2(0, _rb2D.linearVelocity.y);
            }
        }

        // espelha o sprite
        if (moveInput > 0) _spriteRenderer.flipX = false;
        else if (moveInput < 0) _spriteRenderer.flipX = true;

        // executa o pulo
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && _isGrounded)
        {
            _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x, jumpSpeed);
        }
    }

    // controla o animator
    private void UpdateAnimations()
    {
        if (_animator != null)
        {
            // envia a velocidade absoluta para o parametro speed
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
            // aciona o flash e reseta para o inicio da fase
            if (flashEffectScript != null)
            {
                flashEffectScript.TriggerFlash();
            }
            transform.position = _posicaoInicial;
            _rb2D.linearVelocity = Vector2.zero; 
        }
        else if (collision.CompareTag("NextLevel"))
        {
            // carrega a proxima cena da lista
            int proximaCena = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(proximaCena);
        }
        else if (collision.CompareTag("BacktoMenu"))
        {
            SceneManager.LoadScene("menu");
        }
        // se coletou o endereco de memoria, destroi o objeto
        else if (collision.CompareTag("MemoryAdress"))
        {
            Destroy(collision.gameObject);
        }
    }
}