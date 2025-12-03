using UnityEngine;

public class WindPhysics : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private float force = 50f; 
    [SerializeField] private float maxSpeed = 5f; 

    private Transform _fanParent; 

    private void Start()
    {
        // tenta encontrar o objeto pai na hierarquia
        _fanParent = transform.parent;

        // se o objeto estiver solto, avisa e usa a si mesmo para evitar erros
        if (_fanParent == null)
        {
            Debug.LogError("ERRO: O objeto Wind precisa ser filho de um objeto Fan!");
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
                // pega a direcao "cima" do pai (se o pai girar, o vento gira junto)
                Vector2 direcaoVento = _fanParent.up;

                // calculo (produto escalar) pra saber a velocidade do player na direcao do vento
                float velocidadeNaDirecaoDoVento = Vector2.Dot(rb.linearVelocity, direcaoVento);

                // so aplica forca se nao tiver atingido a velocidade maxima
                if (velocidadeNaDirecaoDoVento < maxSpeed)
                {
                    rb.AddForce(direcaoVento * force);
                }
            }
        }
    }

}