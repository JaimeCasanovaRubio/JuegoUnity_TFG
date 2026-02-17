using UnityEngine;
using UnityEngine.EventSystems;

public class PersistEventSys : MonoBehaviour
{
    public static PersistEventSys Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}