using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneChanger : MonoBehaviour
{
    public void gotoControls(){
        GameManager.Instance.ChangeScene("ControlsConfig");
    }
    public void goToMainMenu(){
        GameManager.Instance.ChangeScene(GameManager.Instance.mainMenuScene);
    }
    public void goToSettings(){
        GameManager.Instance.ChangeScene("SettingsMenu");
    }
    public void goToLastScene(){
        if(GameManager.Instance.lastPlayScene!=null &&
           GameManager.Instance.currentScene==GameManager.Instance.settingsScene){
            GameManager.Instance.ChangeScene(GameManager.Instance.lastPlayScene);
        }else{
            GameManager.Instance.ChangeScene(GameManager.Instance.lastScene);
        }
    }
    public void goToSpecific(string sceneName){
        GameManager.Instance.ChangeScene(sceneName);
    }
    
}
