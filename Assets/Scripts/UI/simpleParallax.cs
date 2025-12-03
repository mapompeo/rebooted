using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private GameObject cam; 
    
    // 0.5 move na metade da velocidade, 0.9 quase nao sai do lugar (parece longe)
    [Range(0f, 1f)]
    [SerializeField] private float velocidade = 0.5f; 

    private float _startPos;
    private float _startCamPos;

    void Start()
    {
        if (cam == null) cam = Camera.main.gameObject;
        
        // guarda onde o fundo e a camera comecaram o jogo
        _startPos = transform.position.x;
        _startCamPos = cam.transform.position.x;
    }

    void Update()
    {
        // calcula quanto a camera andou desde o inicio
        float distanciaQueCameraAndou = cam.transform.position.x - _startCamPos;

        // calcula onde o fundo deveria estar (anda so uma fração do que a camera andou)
        float novaPosicaoX = _startPos + (distanciaQueCameraAndou * velocidade);

        // aplica
        transform.position = new Vector3(novaPosicaoX, transform.position.y, transform.position.z);
    }
}