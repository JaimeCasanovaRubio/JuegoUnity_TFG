using UnityEngine;

public class Teleport:MonoBehaviour
{
    
    [SerializeField] int Index;
    public int index => Index;
    [SerializeField] bool GoBack;
    public bool goBack 
    { 
        get => GoBack; 
        set => GoBack = value; 
    }

      
    [SerializeField] private string targetScene;
    public string sceneName 
    { 
        get => targetScene; 
        set => targetScene = value; 
    }

    [SerializeField] private string targetId;
    public string targetRoomId
    {
        get => targetId;
        set => targetId = value;
    }
}