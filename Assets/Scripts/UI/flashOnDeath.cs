using System.Collections;
using UnityEngine;
using UnityEngine.UI; // 1. IMPORTANTE: Adicionei isso pra mexer com Imagem

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
        StopAllCoroutines(); // Para flashs anteriores
        StartCoroutine(DoFadeOutRoutine());
    }

    private IEnumerator DoFadeOutRoutine()
    {
        // 1. Começo do Flash: Liga o painel e deixa totalmente branco
        whitePanelImage.gameObject.SetActive(true);
        SetAlpha(1f);

        // 2. Loop do Fade Out: Vai diminuindo a transparência aos poucos
        float timer = 0f;
        while (timer < fadeDuration)
        {
            // Conta o tempo
            timer += Time.deltaTime;
            
            // Calcula a nova transparência (de 1 até 0) baseada no tempo que passou
            float novaTransparencia = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            SetAlpha(novaTransparencia);

            // Espera até o próximo frame para continuar o loop
            yield return null; 
        }

        // 3. Fim do Flash: Garante que ficou transparente e desliga o objeto
        SetAlpha(0f);
        whitePanelImage.gameObject.SetActive(false);
    }

    // Funçãozinha auxiliar só para facilitar mudar o Alpha
    private void SetAlpha(float alpha)
    {
        Color corAtual = whitePanelImage.color;
        corAtual.a = alpha;
        whitePanelImage.color = corAtual;
    }
}