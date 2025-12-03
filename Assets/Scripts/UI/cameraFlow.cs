using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       
    public float smoothSpeed = 0.005f;  // quanto menor, mais suave/atrasada a camera
    public Vector3 offset;         

    // usa lateupdate pra camera so mexer DEPOIS que o player terminou o movimento dele
    // isso evita tremedeira visual
    void LateUpdate()
    {
        if (player != null)
        {
            // calcula onde a camera quer chegar
            Vector3 desiredPosition = player.position + offset;

            // lerp faz ela ir deslizando suavemente ate la em vez de teleportar
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // aplica a posicao
            transform.position = smoothedPosition;
        }
    }
}