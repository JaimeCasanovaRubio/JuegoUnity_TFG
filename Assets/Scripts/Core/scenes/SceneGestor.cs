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

    //ARRAY DE MAPAS
    public static List<string>  spawns = new List<string>{GameManager.Instance.baseScene, GameManager.Instance.mapaPrueba};
    public static List<MapScene> mpVisited = new List<MapScene>{};

    public static MapScene SavedMap;




    public static void ChangeScene(string sceneName, int index = 5)
    {   
        int random = rnm.Next(0,GameManager.Instance.primerMapa.Count);
        if(spawns.Contains(sceneName))
        {
            MapScene existing = mpVisited.FirstOrDefault(m => m.sceneName == sceneName);
            SavedMap = existing != null ? existing : new MapScene(sceneName);
            LoadScene(sceneName, index);
        }else if(sceneName.Equals("random1"))
        {   
            string sceneToChange = GameManager.Instance.primerMapa[random];
            SavedMap = new MapScene(sceneToChange);
            LoadScene(sceneToChange, index);
        }else if(GameManager.Instance.primerMapa.Contains(sceneName) || mpVisited.Any(m => m.sceneName == sceneName))
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
            if(index == 0 && tp.index == 2)
            {
                tp.goBack = true;
                indexToTP = 2;
                SavedMap.sceneIndex2 = lastMap;
                SavedMap.isVisited2 = true;
                tp.sceneName = lastMap;
            }
            else if(index == 1 && tp.index == 3)
            {   
                tp.goBack = true;
                indexToTP = 3;
                SavedMap.sceneIndex3 = lastMap;
                SavedMap.isVisited3 = true;
                tp.sceneName = lastMap;
            }   
            else if(index == 2 && tp.index == 0)
            {
                tp.goBack = true;
                indexToTP = 0;
                SavedMap.sceneIndex0 = lastMap;
                SavedMap.isVisited0 = true;
                tp.sceneName = lastMap;
            }
            else if(index == 3 && tp.index == 1)
            {   
                tp.goBack = true;
                indexToTP = 1;
                SavedMap.sceneIndex1 = lastMap;
                SavedMap.isVisited1 = true;
                tp.sceneName = lastMap;
            }
        }

        MapScene lastSceneMap = mpVisited.FirstOrDefault(m => m.sceneName == lastMap);
        if (lastSceneMap == null && !string.IsNullOrEmpty(lastMap))
        {
            lastSceneMap = new MapScene(lastMap);
            mpVisited.Add(lastSceneMap);
        }
        if (lastSceneMap != null)
        {
            if (index == 0) { lastSceneMap.sceneIndex0 = SavedMap.sceneName; lastSceneMap.isVisited0 = true; }
            if (index == 1) { lastSceneMap.sceneIndex1 = SavedMap.sceneName; lastSceneMap.isVisited1 = true; }
            if (index == 2) { lastSceneMap.sceneIndex2 = SavedMap.sceneName; lastSceneMap.isVisited2 = true; }
            if (index == 3) { lastSceneMap.sceneIndex3 = SavedMap.sceneName; lastSceneMap.isVisited3 = true; }
        }

        foreach(MapScene scene in mpVisited)
        {
            if(scene.sceneName == SavedMap.sceneName)
            {
                if(scene.sceneIndex0 != "random1"){
                    SavedMap.sceneIndex0 = scene.sceneIndex0;
                    SavedMap.isVisited0 = scene.isVisited0;
                }
                if(scene.sceneIndex1 != "random1"){
                    SavedMap.sceneIndex1 = scene.sceneIndex1;
                    SavedMap.isVisited1 = scene.isVisited1;
                }
                if(scene.sceneIndex2 != "random1"){
                    SavedMap.sceneIndex2 = scene.sceneIndex2;
                    SavedMap.isVisited2 = scene.isVisited2;
                }
                if(scene.sceneIndex3 != "random1"){
                    SavedMap.sceneIndex3 = scene.sceneIndex3;
                    SavedMap.isVisited3 = scene.isVisited3;
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