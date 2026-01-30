using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Pantalla de selección de personaje.
/// Equivalente a CharacterSelectionScreen.java de LibGDX.
/// </summary>
public class CharacterSelection : MonoBehaviour
{
    [Header("Character Buttons")]
    [SerializeField] private Button hurtadillaButton;
    // Añadir más botones para otros personajes aquí

    [Header("Character Prefabs")]
    [SerializeField] private GameObject hurtadillaPrefab;
    // Añadir más prefabs para otros personajes aquí

    [Header("Scene to Load")]
    [SerializeField] private string gameSceneName = "OniricForest";

    [Header("Character Preview")]
    [SerializeField] private Image characterPreviewImage;
    [SerializeField] private TMPro.TextMeshProUGUI characterNameText;
    [SerializeField] private TMPro.TextMeshProUGUI characterDescriptionText;

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

        // Actualizar preview
        if (characterNameText != null)
        {
            characterNameText.text = characterName;
        }

        if (characterDescriptionText != null)
        {
            switch (characterName)
            {
                case "Hurtadilla":
                    characterDescriptionText.text = "Habilidad: DASH\nSe mueve rápidamente en una dirección.";
                    break;
                default:
                    characterDescriptionText.text = "Selecciona un personaje para ver su descripción.";
                    break;
            }
        }

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
        }
        else
        {
            SceneManager.LoadScene(gameSceneName);
        }
    }

    /// <summary>
    /// Vuelve al menú principal.
    /// </summary>
    public void GoBack()
    {
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
