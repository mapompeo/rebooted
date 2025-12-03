using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void IniciarJogo()
    {
        // carrega a cena de introducao
        SceneManager.LoadScene("intro");
    }
}