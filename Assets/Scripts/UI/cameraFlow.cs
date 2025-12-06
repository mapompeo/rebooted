using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;       
    public float smoothSpeed = 0.005f;  
    public Vector3 offset;         

    // variaveis internas pra controlar a tremedeira
    private float _tempoShakeRestante;
    private float _forcaShake;

    // funcao publica pra outros scripts chamarem quando quiserem tremer a tela
    public void TriggerShake(float duracao, float magnitude)
    {
        _tempoShakeRestante = duracao;
        _forcaShake = magnitude;
    }

    // usa lateupdate pra camera so mexer depois que o player terminou o movimento dele
    void LateUpdate()
    {
        if (player != null)
        {
            // calcula onde a camera quer chegar
            Vector3 desiredPosition = player.position + offset;

            // lerp faz ela ir deslizando suavemente ate la em vez de teleportar
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // se ainda tem tempo de shake sobrando, aplica o ruido aleatorio
            if (_tempoShakeRestante > 0)
            {
                // gera um ponto aleatorio dentro de um circulo e multiplica pela forca
                Vector2 shakeOffset = Random.insideUnitCircle * _forcaShake;
                
                // soma esse offset na posicao final da camera
                smoothedPosition.x += shakeOffset.x;
                smoothedPosition.y += shakeOffset.y;

                // diminui o tempo restante
                _tempoShakeRestante -= Time.deltaTime;
            }

            // aplica a posicao final
            transform.position = smoothedPosition;
        }
    }
}