using UnityEngine;

public class CapacitorHead : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private float forcaDoPulo = 8f; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        // verifica se o player caiu na cabeca
        if (other.CompareTag("Player"))
        {
            // impulsiona o player para cima
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, forcaDoPulo);
            }

            // destroi o objeto pai (inimigo completo)
            Destroy(transform.parent.gameObject);
        }
    }
}