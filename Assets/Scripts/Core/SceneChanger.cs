using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneChanger : MonoBehaviour
{
    public void goToMainMenu(){
        GameManager.Instance.ChangeScene(GameManager.Instance.mainMenuScene);
    }
    public void goToSpecific(string sceneName){
        GameManager.Instance.ChangeScene(sceneName);
    }
    
}
