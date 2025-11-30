using UnityEngine;

public class CapacitorHead : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private float forcaDoPulo = 8f; // O quanto o player quica

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se foi o Player que caiu na cabeça
        if (other.CompareTag("Player"))
        {
            // 1. Faz o Player quicar (Bounce)
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Zera a velocidade atual e joga pra cima
                playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, forcaDoPulo);
            }

            // 2. Destroi o Inimigo Pai (O Capacitor inteiro)
            // transform.parent.gameObject pega o objeto Pai
            Destroy(transform.parent.gameObject);
        }
    }
}