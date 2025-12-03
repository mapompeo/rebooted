using UnityEngine;
using UnityEngine.SceneManagement;

public class spaceToContinue : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // pega o indice da cena atual, soma 1 e carrega a proxima
            int proximaCena = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(proximaCena);
        }
    }
}