using UnityEngine;
using UnityEngine.UI;

public class MagicBook : MonoBehaviour
{
    [Header("Armazones")]
    [SerializeField] private Button armazon1; // Verdugo de Titanes
    [SerializeField] private Button armazon2; // Garras de Umbra
    [SerializeField] private Button armazon3; // El Alambique

    [Header("Afinidades (toggle - puede no haber ninguna)")]
    [SerializeField] private Button afinidad1; // AF1
    [SerializeField] private Button afinidad2; // AF2
    [SerializeField] private Button afinidad3; // AF3

    [Header("Retrato derecho del libro")]
    [SerializeField] private Image portraitImage;

    private void OnEnable()
    {
        if (Player.Instance == null) return;

        armazon1.gameObject.SetActive(Player.Instance.Armazon1);
        armazon2.gameObject.SetActive(Player.Instance.Armazon2);
        armazon3.gameObject.SetActive(Player.Instance.Armazon3);

        afinidad1.gameObject.SetActive(Player.Instance.Afinidad1);
        afinidad2.gameObject.SetActive(Player.Instance.Afinidad2);
        afinidad3.gameObject.SetActive(Player.Instance.Afinidad3);

        RefreshPortrait();
    }

    private void RefreshPortrait()
    {
        if (portraitImage == null || Player.Instance == null) return;
        Sprite sprite = Player.Instance.CurrentDesignSprite;
        if (sprite != null) portraitImage.sprite = sprite;
    }

    public void OnVolverClicker()
    {
        GameManager.Instance.MagicBookCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnArmazon1Clicker()
    {
        Player.Instance.Armazon = "Armazon1";
        RefreshPortrait();
    }
    public void OnArmazon2Clicker()
    {
        Player.Instance.Armazon = "Armazon2";
        RefreshPortrait();
    }
    public void OnArmazon3Clicker()
    {
        Player.Instance.Armazon = "Armazon3";
        RefreshPortrait();
    }

    // Las afinidades son toggle: si ya está seleccionada, se deselecciona (vuelve a sprite base)
    public void OnAfinidad1Clicker()
    {
        Player.Instance.Afinidad = Player.Instance.Afinidad == "Afinidad1" ? "" : "Afinidad1";
        RefreshPortrait();
    }
    public void OnAfinidad2Clicker()
    {
        Player.Instance.Afinidad = Player.Instance.Afinidad == "Afinidad2" ? "" : "Afinidad2";
        RefreshPortrait();
    }
    public void OnAfinidad3Clicker()
    {
        Player.Instance.Afinidad = Player.Instance.Afinidad == "Afinidad3" ? "" : "Afinidad3";
        RefreshPortrait();
    }
}