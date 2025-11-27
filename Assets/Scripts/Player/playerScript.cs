using UnityEngine;

public class playerScript : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float jumpSpeed = 5f;
    [SerializeField] private float moveSpeed = 5f;
    
    [Header("Verificação de Chão")]
    [SerializeField] private Transform groundCheck; // O objeto vazio que fica no pé
    [SerializeField] private LayerMask groundLayer; // O que é considerado "Chão"
    
    private Rigidbody2D _rb2D;
    private bool _isGrounded; // Variável para saber se pode pular

    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Verifica se existe chão num raio de 0.2 unidades na posição do  groundCheck
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        Movimentation();
    }

    private void Movimentation()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        _rb2D.linearVelocity = new Vector2(moveInput * moveSpeed, _rb2D.linearVelocity.y);

        // Adicionei o "&& _isGrounded" aqui
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && _isGrounded)
        {
            _rb2D.linearVelocity = new Vector2(_rb2D.linearVelocity.x, jumpSpeed);
        }
    }
    
    // Essa função desenha uma bolinha vermelha na cena pra você ver onde está o sensor
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player Collided");
    }
}