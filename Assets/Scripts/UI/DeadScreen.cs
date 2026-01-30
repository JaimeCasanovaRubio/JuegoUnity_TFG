using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Pantalla de muerte.
/// Equivalente a DeadScreen.java de LibGDX.
/// </summary>
public class DeadScreen : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TMPro.TextMeshProUGUI gameOverText;

    [Header("Animation")]
    [SerializeField] private float fadeInDuration = 1f;
    [SerializeField] private CanvasGroup canvasGroup;

    private void Start()
    {
        // Configurar listeners
        if (retryButton != null)
            retryButton.onClick.AddListener(Retry);
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);

        // Animación de fade in
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            StartCoroutine(FadeIn());
        }
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Clamp01(elapsed / fadeInDuration);
            }
            yield return null;
        }
    }

    /// <summary>
    /// Reintenta el nivel actual.
    /// </summary>
    public void Retry()
    {
        Debug.Log("🔄 Reintentando...");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GoToLastPlayScene();
        }
        else
        {
            // Recargar la escena anterior guardada o una por defecto
            string lastScene = PlayerPrefs.GetString("LastPlayScene", "OniricForest");
            SceneManager.LoadScene(lastScene);
        }
    }

    /// <summary>
    /// Vuelve al menú principal.
    /// </summary>
    public void GoToMainMenu()
    {
        Debug.Log("🏠 Volviendo al menú principal...");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeScene("MainMenu");
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
