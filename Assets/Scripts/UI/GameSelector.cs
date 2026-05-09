using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


/// <summary>
/// Pantalla de selección de juego.
/// </summary>
public class GameSelector : MonoBehaviour
{
    [Header("Game Buttons")]
    [SerializeField] private Button partida1;
    [SerializeField] private Button partida2;
    
    public static int gameSelected = 0;
    

    [Header("Navigation")]
    public UnityEvent onBackEvent = new UnityEvent();

    private void Start()
    {
        var selectCharUI = GameManager.Instance.SelectCharCanvas?.GetComponentInChildren<CharacterSelection>(true);
        if (selectCharUI != null)
        {
            selectCharUI.onBackEvent.RemoveListener(OnBackFromMenu);
            selectCharUI.onBackEvent.AddListener(OnBackFromMenu);

        }
    }

    /// <summary>
    /// Selecciona un juego.
    /// </summary>
    public void SelectGame(string gameName)
    {   
        if(gameName == "game1"){
            gameSelected = 1; // IMPORTANTE: Hay que guardarlo también si la partida ya existe
            if(PlayerPrefs.GetInt("Game1") == 1){
                GameManager.Instance.CreateOnPlayerPrefs(1);
            }else{
                if(GameManager.Instance.SelectCharCanvas!=null){
                    GameManager.Instance.SelectCharCanvas.SetActive(true);
                    GameManager.Instance.GameSelectorCanvas.SetActive(false);
                }
                else {
                    Debug.LogError("[GameSelector] No se pudo encontrar el canvas de Selección de Personaje persistente.");
                }
            }
            
        }
        else if(gameName == "game2"){
            gameSelected = 2; // IMPORTANTE: Hay que guardarlo también si la partida ya existe
            if(PlayerPrefs.GetInt("Game2") == 1){
                GameManager.Instance.CreateOnPlayerPrefs(2);
            }else{
                if(GameManager.Instance.SelectCharCanvas!=null){
                    GameManager.Instance.SelectCharCanvas.SetActive(true);
                    GameManager.Instance.GameSelectorCanvas.SetActive(false);
                }
                else {
                    Debug.LogError("[GameSelector] No se pudo encontrar el canvas de Selección de Personaje persistente.");
                }
            }
        }
    }

    private void OnBackFromMenu()
    {
        if(GameManager.Instance.SelectCharCanvas != null) GameManager.Instance.SelectCharCanvas.SetActive(false);
        if(GameManager.Instance.GameSelectorCanvas != null) GameManager.Instance.GameSelectorCanvas.SetActive(true);
    }
    /// <summary>
    /// Vuelve al menú principal.
    /// </summary>
    public void GoBack()
    {
        onBackEvent?.Invoke();
    }
}
