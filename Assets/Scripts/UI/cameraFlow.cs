using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       // O alvo (seu boneco)
    public float smoothSpeed = 0.005f;  // A suavidade do movimento (0 a 1)
    public Vector3 offset;         // A distância que a câmera mantém

    // Usamos LateUpdate porque queremos que a câmera mova SÓ DEPOIS
    // que o player já tiver terminado de se mexer naquele frame.
    // Isso evita tremedeira.
    void LateUpdate()
    {
        if (player != null)
        {
            // Onde a câmera quer estar (Posição do player + a distância original)
            Vector3 desiredPosition = player.position + offset;

            // O Lerp faz ela ir "deslizando" suavemente até lá em vez de teleportar
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Aplica a posição
            transform.position = smoothedPosition;
        }
    }
}