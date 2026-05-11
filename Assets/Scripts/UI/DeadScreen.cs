using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DeadScreen : MonoBehaviour
{
    public void OnControlsClicked()
    {
        var controlsUI = GameManager.Instance.ControlsCanvas?.GetComponentInChildren<ControlsConfigUI>(true);
        controlsUI.onBackEvent.RemoveAllListeners();
        controlsUI.onBackEvent.AddListener(OnControlsBackClicked);


        GameManager.Instance.DeadScreenCanvas.SetActive(false);
        GameManager.Instance.ControlsCanvas.SetActive(true);
    }
    private void OnControlsBackClicked()
    {
        GameManager.Instance.ControlsCanvas.SetActive(false);
        GameManager.Instance.DeadScreenCanvas.SetActive(true);
    }

    public void OnVolverBaseClicked()
    {
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            Destroy(player.gameObject);
            Player.Instance = null; // Libera la instancia para que la nueva se convierta en la principal
        }

        GameManager.Instance.DeadScreenCanvas.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.CreateOnPlayerPrefs(GameSelector.gameSelected);
        SceneGestor.ClearMapVisited();
    }

    public void OnVolverClicker()
    {
        GameManager.Instance.DeadScreenCanvas.SetActive(false);
        GameManager.Instance.ChangeScene("MenuScene");
    }
}