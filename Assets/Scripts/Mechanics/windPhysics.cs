using UnityEngine;

public class WindPhysics : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float force = 50f; 
    [SerializeField] private float maxSpeed = 5f; 

    // Variável privada (não aparece no Inspector) para guardar o Pai
    private Transform _fanParent; 

    private void Start()
    {
        // Pega o objeto imediatamente acima na hierarquia (o Fan)
        _fanParent = transform.parent;

        // Segurança: Se você colocar esse script num objeto solto sem pai, ele avisa
        if (_fanParent == null)
        {
            Debug.LogError("ERRO: O objeto Wind precisa ser filho de um objeto Fan!");
            // Se não tiver pai, usa a si mesmo pra não travar o jogo
            _fanParent = transform; 
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                // Usa o "Cima" do Pai (_fanParent) para decidir a direção
                // Se o Pai girar, o vento gira junto.
                Vector2 direcaoVento = _fanParent.up;

                float velocidadeNaDirecaoDoVento = Vector2.Dot(rb.linearVelocity, direcaoVento);

                if (velocidadeNaDirecaoDoVento < maxSpeed)
                {
                    rb.AddForce(direcaoVento * force);
                }
            }
        }
    }

}