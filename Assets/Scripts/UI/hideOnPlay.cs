using UnityEngine;

public class HideOnPlay : MonoBehaviour
{
    void Start()
    {
        // usado no Void, para aparecer na tela da cena, mas nao aparecer in-game
        // desliga o sprite (desenho) pra ficar invisivel, mas mantem o collider funcionando
        GetComponent<SpriteRenderer>().enabled = false;
    }
}