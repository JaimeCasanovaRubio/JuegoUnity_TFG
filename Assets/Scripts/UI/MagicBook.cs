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

        // Si lo tiene (true) -> se activa (true). Si no lo tiene (false) -> se desactiva (false).
        armazon1.gameObject.SetActive(Player.Instance.Armazon1);
        armazon2.gameObject.SetActive(Player.Instance.Armazon2);
        armazon3.gameObject.SetActive(Player.Instance.Armazon3);
        
        afinidad1.gameObject.SetActive(Player.Instance.Afinidad1);
        afinidad2.gameObject.SetActive(Player.Instance.Afinidad2);
        afinidad3.gameObject.SetActive(Player.Instance.Afinidad3);
        afinidad4.gameObject.SetActive(Player.Instance.Afinidad4);
    }

    public void OnVolverClicker()
    {
        GameManager.Instance.MagicBookCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnArmazon1Clicker()
    {
        Player.Instance.Armazon = "Armazon1";
    }
    public void OnArmazon2Clicker()
    {
        Player.Instance.Armazon = "Armazon2";
    }
    public void OnArmazon3Clicker()
    {
        Player.Instance.Armazon = "Armazon3";
    }
    public void OnAfinidad1Clicker()
    {
        Player.Instance.Afinidad = "Afinidad1";
    }
    public void OnAfinidad2Clicker()
    {
        Player.Instance.Afinidad = "Afinidad2";
    }
    public void OnAfinidad3Clicker()
    {
        Player.Instance.Afinidad = "Afinidad3";
    }
    public void OnAfinidad4Clicker()
    {
        Player.Instance.Afinidad = "Afinidad4";
    }
}