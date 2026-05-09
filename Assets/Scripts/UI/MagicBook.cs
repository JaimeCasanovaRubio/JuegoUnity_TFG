using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class MagicBook : MonoBehaviour
{
    [Header("Armazones")]
    [SerializeField] private Button armazon1;
    [SerializeField] private Button armazon2;
    [SerializeField] private Button armazon3;
    [Header("Afinidades")]
    [SerializeField] private Button afinidad1;
    [SerializeField] private Button afinidad2;
    [SerializeField] private Button afinidad3;
    [SerializeField] private Button afinidad4;

    private void OnEnable() {
        if (Player.Instance == null) return; // Evita el error al instanciar el prefab en el menú principal

        if(Player.Instance.Armazon1){
            armazon1.interactable = false;
        }
        if(Player.Instance.Armazon2){
            armazon2.interactable = false;
        }
        if(Player.Instance.Armazon3){
            armazon3.interactable = false;
        }
        if(Player.Instance.Afinidad1){
            afinidad1.interactable = false;
        }
        if(Player.Instance.Afinidad2){
            afinidad2.interactable = false;
        }
        if(Player.Instance.Afinidad3){
            afinidad3.interactable = false;
        }
        if(Player.Instance.Afinidad4){
            afinidad4.interactable = false;
        }
    }

    public void OnVolverClicker()
    {
        GameManager.Instance.MagicBookCanvas.SetActive(false);
        GameManager.Instance.TogglePause();
    }
}