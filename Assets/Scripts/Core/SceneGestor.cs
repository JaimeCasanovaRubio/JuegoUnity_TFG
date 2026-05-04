using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneGestor
{   

    private static System.Random rnm = new System.Random();
    private static string lastMap;
    public static int doorIndex;
    public static int indexToTP;
    //NOMBRES ESCENAS DE MAPA
    private static string mp1 = "MP1";
    private static string mp2 = "MP2";
    private static string mp3 = "MP3";
    private static string mp4 = "MP4";
    private static string mp5 = "MP5";

    //ARRAY DE MAPAS
    public static List<string>  spawns = new List<string>{GameManager.baseScene, GameManager.mapaPrueba};
    public static List<string> mp = new List<string>{mp1, mp2, mp3, mp4, mp5};
    public static List<MapScene> mpVisited = new List<MapScene>{};

    public static MapScene SavedMap;




    public static void ChangeScene(string sceneName, int index = 5)
    {   
        int random = rnm.Next(0,mp.Count);
        if(spawns.Contains(sceneName))
        {
            LoadScene(sceneName);
        }else if(sceneName.Equals("random1"))
        {   
            string sceneToChange = mp[random];
            SavedMap = new MapScene(sceneToChange);
            //mp.RemoveAt(random);
            LoadScene(sceneToChange, index);
        }else if(mp.Contains(sceneName) || mpVisited.Any(m => m.sceneName == sceneName))
        {
            MapScene existing = mpVisited.FirstOrDefault(m => m.sceneName == sceneName);
            SavedMap = existing != null ? existing : new MapScene(sceneName);
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
                    indexToTP = 2;
                    SavedMap.sceneIndex0 = lastMap;
                    tp.sceneName = lastMap;
                }
            }
            else if(index == 1)
            {
                if(tp.index == 3)
                {   
                    indexToTP = 3;
                    SavedMap.sceneIndex1 = lastMap;
                    tp.sceneName = lastMap;
                }
            }   
            else if(index == 2)
            {
                if(tp.index == 0)
                {
                    indexToTP = 0;
                    SavedMap.sceneIndex2 = lastMap;
                    tp.sceneName = lastMap;
                }
            }
            else if(index == 3)
            {   
                if(tp.index == 1)
                {
                    indexToTP = 1;
                    SavedMap.sceneIndex3 = lastMap;
                    tp.sceneName = lastMap;
                }
            }
        }
        foreach(MapScene scene in mpVisited)
        {
            if(scene.sceneName == SavedMap.sceneName)
            {
                if(scene.sceneIndex0 != "random1"){
                    SavedMap.sceneIndex0 = scene.sceneIndex0;
                }
                if(scene.sceneIndex1 != "random1"){
                    SavedMap.sceneIndex1 = scene.sceneIndex1;
                }
                if(scene.sceneIndex2 != "random1"){
                    SavedMap.sceneIndex2 = scene.sceneIndex2;
                }
                if(scene.sceneIndex3 != "random1"){
                    SavedMap.sceneIndex3 = scene.sceneIndex3;
                }
                mpVisited.Remove(scene);
                break;
            }
        }
        mpVisited.Add(SavedMap);

    }
    private static void LoadScene(string sceneName, int index = 5)
    {   
        lastMap = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        doorIndex = index;
        SceneManager.LoadScene(sceneName);
    }

}