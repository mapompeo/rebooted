using UnityEngine;

public class HideOnPlay : MonoBehaviour
{
    void Start()
    {
        // Desliga o desenho (Sprite), mas mantem a parede fisica (Collider)
        GetComponent<SpriteRenderer>().enabled = false;
    }
}