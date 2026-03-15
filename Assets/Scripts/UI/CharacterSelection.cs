using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


/// <summary>
/// Pantalla de selección de personaje.
/// Equivalente a CharacterSelectionScreen.java de LibGDX.
/// </summary>
public class CharacterSelection : MonoBehaviour
{
    [Header("Character Buttons")]
    [SerializeField] private Button hurtadillaButton;
    // Añadir más botones para otros personajes aquí

    [Header("Scene to Load")]
    [SerializeField] private string gameSceneName = "Base";

    [Header("Character Preview")]
    [SerializeField] private Image characterPreviewImage;
    [SerializeField] private TMPro.TextMeshProUGUI characterNameText;
    [SerializeField] private TMPro.TextMeshProUGUI characterDescriptionText;

    [Header("Navigation")]
    public UnityEvent onBackEvent = new UnityEvent();

    private string selectedCharacter = "";

    private void Start()
    {
        // Configurar listeners
        if (hurtadillaButton != null)
        {
            hurtadillaButton.onClick.AddListener(() => SelectCharacter("Hurtadilla"));
        }
    }

    /// <summary>
    /// Selecciona un personaje.
    /// </summary>
    public void SelectCharacter(string characterName)
    {
        selectedCharacter = characterName;
        Debug.Log($"🎭 Personaje seleccionado: {characterName}");

        // Iniciar el juego con este personaje
        StartGameWithCharacter(characterName);
    }

    /// <summary>
    /// Inicia el juego con el personaje seleccionado.
    /// </summary>
    private void StartGameWithCharacter(string characterName)
    {
        // Guardar el personaje seleccionado en PlayerPrefs
        PlayerPrefs.SetString("SelectedCharacter", characterName);
        PlayerPrefs.Save();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGameWithCharacter(characterName);
            if (GameManager.Instance.SelectCharCanvas != null) {
                GameManager.Instance.SelectCharCanvas.SetActive(false);
            }
        }
        else
        {   
            Debug.LogWarning("⚠️ GameManager no encontrado. Cargando escena 'Base' directamente.");
            SceneManager.LoadScene("Base");
        }
    }

    /// <summary>
    /// Vuelve al menú principal.
    /// </summary>
    public void GoBack()
    {
        onBackEvent?.Invoke();
    }
}
