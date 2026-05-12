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




    public static MapScene lastSceneMap;

    public static void ChangeScene(string sceneName, int index = 5)
    {   
        int random = rnm.Next(0,GameManager.Instance.primerMapa.Count);
        lastSceneMap = SavedMap;
        if(sceneName.Equals("Boss")){
            SavedMap = null;
            LoadScene(sceneName, index);
        }

        if(spawns.Contains(sceneName))
        {
            SavedMap = null;
            LoadScene(sceneName, index);
        }else if(sceneName.Equals("random1"))
        {   
            string sceneToChange = GameManager.Instance.primerMapa[random];
            SavedMap = new MapScene(sceneToChange);
            LoadScene(sceneToChange, index);
        }else if(GameManager.Instance.primerMapa.Contains(sceneName))
        {
            MapScene existing = mpVisited.FirstOrDefault(m => m.sceneName == sceneName);
            SavedMap = existing != null ? existing : new MapScene(sceneName);
            LoadScene(sceneName,index);
        }
    }
    public static void ChangeScene(string sceneName, string roomId,int index = 5)
    {
        lastSceneMap = SavedMap;
        MapScene existing = mpVisited.FirstOrDefault(m => m.roomId == roomId);
        SavedMap = existing != null ? existing : new MapScene(sceneName);
        LoadScene(sceneName,index);
    }
    public static void SetLastScene(int index)
    {   
        if(SceneManager.GetActiveScene().name.Equals("Boss")) return;
        
        Teleport[] tps = Object.FindObjectsByType<Teleport>(FindObjectsSortMode.None);

        if (lastSceneMap != null && !mpVisited.Contains(lastSceneMap))
        {
            mpVisited.Add(lastSceneMap);
        }

        foreach (Teleport tp in tps)
        {
            if(index == 0 && tp.index == 2)
            {
                tp.goBack = true;
                indexToTP = 2;
                if (lastSceneMap != null)
                {
                    SavedMap.sceneIndex2 = lastSceneMap.sceneName;
                    SavedMap.roomId2 = lastSceneMap.roomId;
                    SavedMap.isVisited2 = true;
                    tp.sceneName = lastSceneMap.sceneName;
                    tp.targetRoomId = lastSceneMap.roomId;
                }
            }
            else if(index == 1 && tp.index == 3)
            {   
                tp.goBack = true;
                indexToTP = 3;
                if (lastSceneMap != null)
                {
                    SavedMap.sceneIndex3 = lastSceneMap.sceneName;
                    SavedMap.roomId3 = lastSceneMap.roomId;
                    SavedMap.isVisited3 = true;
                    tp.sceneName = lastSceneMap.sceneName;
                    tp.targetRoomId = lastSceneMap.roomId;
                }
            }   
            else if(index == 2 && tp.index == 0)
            {
                tp.goBack = true;
                indexToTP = 0;
                if (lastSceneMap != null)
                {
                    SavedMap.sceneIndex0 = lastSceneMap.sceneName;
                    SavedMap.roomId0 = lastSceneMap.roomId;
                    SavedMap.isVisited0 = true;
                    tp.sceneName = lastSceneMap.sceneName;
                    tp.targetRoomId = lastSceneMap.roomId;
                }
            }
            else if(index == 3 && tp.index == 1)
            {   
                tp.goBack = true;
                indexToTP = 1;
                if (lastSceneMap != null)
                {
                    SavedMap.sceneIndex1 = lastSceneMap.sceneName;
                    SavedMap.roomId1 = lastSceneMap.roomId;
                    SavedMap.isVisited1 = true;
                    tp.sceneName = lastSceneMap.sceneName;
                    tp.targetRoomId = lastSceneMap.roomId;
                }
            }
        }

        if (lastSceneMap != null)
        {
            if (index == 0) { lastSceneMap.sceneIndex0 = SavedMap.sceneName; lastSceneMap.roomId0 = SavedMap.roomId; lastSceneMap.isVisited0 = true; }
            if (index == 1) { lastSceneMap.sceneIndex1 = SavedMap.sceneName; lastSceneMap.roomId1 = SavedMap.roomId; lastSceneMap.isVisited1 = true; }
            if (index == 2) { lastSceneMap.sceneIndex2 = SavedMap.sceneName; lastSceneMap.roomId2 = SavedMap.roomId; lastSceneMap.isVisited2 = true; }
            if (index == 3) { lastSceneMap.sceneIndex3 = SavedMap.sceneName; lastSceneMap.roomId3 = SavedMap.roomId; lastSceneMap.isVisited3 = true; }
        }

        if (SavedMap != null && !mpVisited.Contains(SavedMap))
        {
            mpVisited.Add(SavedMap);
        }
    }
    public static void ClearMapVisited(){
        mpVisited.Clear();
    }
    private static void LoadScene(string sceneName, int index = 5)
    {   
        lastMap = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        doorIndex = index;
        SceneManager.LoadScene(sceneName);
    }

}