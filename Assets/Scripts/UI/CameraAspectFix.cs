using UnityEngine;

public class CameraAspectFix : MonoBehaviour
{
    // define a proporcao que voce quer (16:9 = 1.7777)
    [SerializeField] private float targetAspect = 16.0f / 9.0f;

    void Start()
    {
        UpdateCrop();
    }

    void Update()
    {
        UpdateCrop();
    }

    void UpdateCrop()
    {
        // calcula a proporcao atual da tela do jogador
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // calcula quanto temos que escalar a altura pra bater com a largura
        float scaleHeight = windowAspect / targetAspect;

        Camera camera = GetComponent<Camera>();

        // se a tela for mais "alta" (estreita) que o jogo (celular em pe, monitor quadrado)
        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f; // centraliza verticalmente

            camera.rect = rect;
        }
        else // se a tela for mais "larga" que o jogo (monitor ultrawide)
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f; // centraliza horizontalmente
            rect.y = 0;

            camera.rect = rect;
        }
    }
}