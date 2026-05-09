

public class MapScene
{
    public string roomId;
    public string sceneName;
    
    public string sceneIndex0 = "random1";
    public string roomId0 = "";
    
    public string sceneIndex1 = "random1";
    public string roomId1 = "";
    
    public string sceneIndex2 = "random1";
    public string roomId2 = "";
    
    public string sceneIndex3 = "random1";
    public string roomId3 = "";

    public bool isVisited0 = false;
    public bool isVisited1 = false;
    public bool isVisited2 = false;
    public bool isVisited3 = false;
    public bool isVisited = false;
    
    public MapScene(string sceneName){
        this.roomId = System.Guid.NewGuid().ToString();
        this.sceneName = sceneName;
    }
}