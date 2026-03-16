using UnityEngine;

public class Teleport:MonoBehaviour
{
    [SerializeField] string targetScene;
    public string sceneName => targetScene;  
}