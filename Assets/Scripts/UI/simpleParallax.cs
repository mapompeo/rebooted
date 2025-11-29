using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private GameObject cam; 
    
    // 0.5 = Metade da velocidade (Bom para fundo)
    // 0.9 = Quase parado (Bom para céu muito distante)
    [Range(0f, 1f)]
    [SerializeField] private float velocidade = 0.5f; 

    private float _startPos;
    private float _startCamPos;

    void Start()
    {
        if (cam == null) cam = Camera.main.gameObject;
        
        // Guarda onde o fundo e a câmera começaram
        _startPos = transform.position.x;
        _startCamPos = cam.transform.position.x;
    }

    void Update()
    {
        // 1. Quanto a câmera andou desde o começo do jogo?
        float distanciaQueCameraAndou = cam.transform.position.x - _startCamPos;

        // 2. O fundo deve andar apenas uma fração disso (ex: 50%)
        float novaPosicaoX = _startPos + (distanciaQueCameraAndou * velocidade);

        // 3. Aplica
        transform.position = new Vector3(novaPosicaoX, transform.position.y, transform.position.z);
    }
}