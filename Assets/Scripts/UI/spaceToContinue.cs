using UnityEngine;
using UnityEngine.SceneManagement;

public class spaceToContinue : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Pega o índice da cena atual e soma 1
            int proximaCena = SceneManager.GetActiveScene().buildIndex + 1;
            
            // Carrega imediatamente
            SceneManager.LoadScene(proximaCena);
        }
    }
}