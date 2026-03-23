using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SettingsConfigUI : MonoBehaviour
{
    public void OnControlsClicked()
    {
        var controlsUI = GameManager.Instance.ControlsCanvas?.GetComponentInChildren<ControlsConfigUI>(true);
        controlsUI.onBackEvent.RemoveAllListeners();
        controlsUI.onBackEvent.AddListener(OnControlsBackClicked);


        GameManager.Instance.SettingsCanvas.SetActive(false);
        GameManager.Instance.ControlsCanvas.SetActive(true);
    }
    private void OnControlsBackClicked()
    {
        GameManager.Instance.ControlsCanvas.SetActive(false);
        GameManager.Instance.SettingsCanvas.SetActive(true);
    }

    public void OnVolverBaseClicked()
    {
        GameManager.Instance.SettingsCanvas.SetActive(false);
        //GameManager.Instance.ChangeView();
        GameManager.Instance.ChangeScene(GameManager.baseScene);
    }

    public void OnVolverClicker()
    {
        GameManager.Instance.SettingsCanvas.SetActive(false);
        //GameManager.Instance.ChangeView();
    }
}