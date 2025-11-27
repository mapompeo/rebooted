using UnityEngine;

public class playerScript : MonoBehaviour
{
    [SerializeField] private float jumpSpeed = 5f;
    
    private Rigidbody2D _rb2D;
    
    
    
    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        Movimentation();
    }

    private void Movimentation()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb2D.linearVelocity = Vector2.zero;
            _rb2D.linearVelocity = Vector2.up * jumpSpeed;
        }
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
