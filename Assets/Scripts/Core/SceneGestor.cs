using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class SceneGestor
{   

    private static System.Random rnm = new System.Random();
    //NOMBRES ESCENAS DE MAPA
    private static string mp1 = "MP1";
    private static string mp2 = "MP2";
    private static string mp3 = "MP3";
    private static string mp4 = "MP4";
    private static string mp5 = "MP5";

    //ARRAY DE MAPAS
    public static string [] spawns = {GameManager.baseScene, GameManager.mapaPrueba};
    public static string[] mp = {mp1, mp2, mp3, mp4, mp5};




    public static void ChangeScene(string sceneName)
    {   
        if(spawns.Contains(sceneName))
        {
            LoadScene(sceneName);
        }else if(sceneName.Equals("random1"))
        {
            LoadScene(mp[rnm.Next(0,mp.Length)]);
        }
    }
    private static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}