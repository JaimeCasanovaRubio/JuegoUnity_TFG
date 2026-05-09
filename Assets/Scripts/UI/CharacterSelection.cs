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
        int gameSelected = GameSelector.gameSelected;

        // Iniciar el juego con este personaje
        StartGameWithCharacter(characterName, gameSelected);
    }

    /// <summary>
    /// Inicia el juego con el personaje seleccionado.
    /// </summary>
    private void StartGameWithCharacter(string characterName, int gameSelected)
    {
        // Guardar el personaje seleccionado en PlayerPrefs
        PlayerPrefs.SetString("SelectedCharacter", characterName);
        PlayerPrefs.Save();
        if (GameManager.Instance != null)
        {
            PlayerPrefs.SetInt("Game"+gameSelected,1);
            
            GameManager.Instance.StartGameWithCharacter(characterName, gameSelected);
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
