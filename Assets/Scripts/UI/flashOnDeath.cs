using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashEffect : MonoBehaviour
{
    [Header("Arrastar o Painel Branco aqui")]
    // Mudei de GameObject para Image, pra poder acessar a cor
    [SerializeField] private Image whitePanelImage;

    [Header("Configurações")]
    [SerializeField] private float fadeDuration = 0.5f; // Tempo para desaparecer
    
    private void Start()
    {
        // Garante que começa invisível e desligado
        if (whitePanelImage != null)
        {
            SetAlpha(0f);
            whitePanelImage.gameObject.SetActive(false);
        }
    }

    public void TriggerFlash()
    {
        StopAllCoroutines(); // Para os flashs anteriores
        StartCoroutine(DoFadeOutRoutine());
    }

    private IEnumerator DoFadeOutRoutine()
    {
        // ativa o painel branco na tela
        whitePanelImage.gameObject.SetActive(true);
        SetAlpha(1f);

        // loop pra ir diminuindo a transparencia aos poucos
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            
            // calcula quanto de transparencia tem que ter agora baseado no tempo que passou
            float novaTransparencia = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            SetAlpha(novaTransparencia);

            // pausa aqui e continua no proximo frame pra dar o efeito visual suave
            yield return null; 
        }

        // garante que ficou 100% transparente e desliga o objeto pra economizar
        SetAlpha(0f);
        whitePanelImage.gameObject.SetActive(false);
    }

    // funçãozinha auxiliar só para facilitar mudar o Alpha
    private void SetAlpha(float alpha)
    {
        Color corAtual = whitePanelImage.color;
        corAtual.a = alpha;
        whitePanelImage.color = corAtual;
    }
}