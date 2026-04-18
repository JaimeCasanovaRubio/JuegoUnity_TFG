using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneGestor
{   

    private static System.Random rnm = new System.Random();
    private static string lastMap;
    public static int doorIndex;
    //NOMBRES ESCENAS DE MAPA
    private static string mp1 = "MP1";
    private static string mp2 = "MP2";
    private static string mp3 = "MP3";
    private static string mp4 = "MP4";
    private static string mp5 = "MP5";

    //ARRAY DE MAPAS
    public static List<string>  spawns = new List<string>{GameManager.baseScene, GameManager.mapaPrueba};
    public static List<string> mp = new List<string>{mp1, mp2, mp3, mp4, mp5};
    public static List<string> mpVisited = new List<string>{};




    public static void ChangeScene(string sceneName, int index = 5)
    {   
        int random = rnm.Next(0,mp.Count);
        if(spawns.Contains(sceneName))
        {
            LoadScene(sceneName);
        }else if(sceneName.Equals("random1"))
        {   
            string sceneToChange = mp[random];
            //mp.RemoveAt(random);
            mpVisited.Add(sceneToChange);
            LoadScene(sceneToChange, index);
        }else if(mp.Contains(sceneName))
        {
            LoadScene(sceneName,index);
        }
    }
    public static void SetLastScene(int index)
    {
        Teleport[] tps = Object.FindObjectsByType<Teleport>(FindObjectsSortMode.None);
        foreach (Teleport tp in tps)
        {
            if(index == 0)
            {
                if(tp.index == 2)
                {
                    tp.sceneName = lastMap;
                }
            }
            else if(index == 1)
            {
                if(tp.index == 3)
                {
                    tp.sceneName = lastMap;
                }
            }   
            else if(index == 2)
            {
                if(tp.index == 0)
                {
                    tp.sceneName = lastMap;
                }
            }
            else if(index == 3)
            {   
                if(tp.index == 1)
                {
                    tp.sceneName = lastMap;
                }
            }
        }
    }
    private static void LoadScene(string sceneName, int index = 5)
    {   
        lastMap = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        doorIndex = index;
        SceneManager.LoadScene(sceneName);
    }

}