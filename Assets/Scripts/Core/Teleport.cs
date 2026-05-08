using UnityEngine;

public class Teleport:MonoBehaviour
{
    
    [SerializeField] int Index;
    public int index => Index;
    [SerializeField] bool GoBack;
    public bool goBack => GoBack;

      
    [SerializeField] private string targetScene;
    public string sceneName 
    { 
        get => targetScene; 
        set => targetScene = value; 
    }
}