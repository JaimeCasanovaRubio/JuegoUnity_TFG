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

    [Header("Círculos de selección (Image hijo de cada botón)")]
    [SerializeField] private Image circleArmazon1;
    [SerializeField] private Image circleArmazon2;
    [SerializeField] private Image circleArmazon3;
    [SerializeField] private Image circleAfinidad1;
    [SerializeField] private Image circleAfinidad2;
    [SerializeField] private Image circleAfinidad3;

    [Header("Retrato derecho del libro")]
    [SerializeField] private Image portraitImage;

    private void OnEnable()
    {
        if (Player.Instance == null) return;

        if (armazon1 != null) armazon1.gameObject.SetActive(Player.Instance.Armazon1);
        if (armazon2 != null) armazon2.gameObject.SetActive(Player.Instance.Armazon2);
        if (armazon3 != null) armazon3.gameObject.SetActive(Player.Instance.Armazon3);

        if (afinidad1 != null) afinidad1.gameObject.SetActive(Player.Instance.Afinidad1);
        if (afinidad2 != null) afinidad2.gameObject.SetActive(Player.Instance.Afinidad2);
        if (afinidad3 != null) afinidad3.gameObject.SetActive(Player.Instance.Afinidad3);

        UpdateSelectionVisuals();
        RefreshPortrait();
    }

    private void UpdateSelectionVisuals()
    {
        if (Player.Instance == null) return;

        if (circleArmazon1 != null) circleArmazon1.gameObject.SetActive(Player.Instance.Armazon == "Armazon1");
        if (circleArmazon2 != null) circleArmazon2.gameObject.SetActive(Player.Instance.Armazon == "Armazon2");
        if (circleArmazon3 != null) circleArmazon3.gameObject.SetActive(Player.Instance.Armazon == "Armazon3");

        if (circleAfinidad1 != null) circleAfinidad1.gameObject.SetActive(Player.Instance.Afinidad == "Afinidad1");
        if (circleAfinidad2 != null) circleAfinidad2.gameObject.SetActive(Player.Instance.Afinidad == "Afinidad2");
        if (circleAfinidad3 != null) circleAfinidad3.gameObject.SetActive(Player.Instance.Afinidad == "Afinidad3");
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
        UpdateSelectionVisuals();
        RefreshPortrait();
    }
    public void OnArmazon2Clicker()
    {
        Player.Instance.Armazon = "Armazon2";
        UpdateSelectionVisuals();
        RefreshPortrait();
    }
    public void OnArmazon3Clicker()
    {
        Player.Instance.Armazon = "Armazon3";
        UpdateSelectionVisuals();
        RefreshPortrait();
    }

    // Las afinidades son toggle: si ya está seleccionada, se deselecciona (vuelve a sprite base)
    public void OnAfinidad1Clicker()
    {
        Player.Instance.Afinidad = Player.Instance.Afinidad == "Afinidad1" ? "Afinidad0" : "Afinidad1";
        UpdateSelectionVisuals();
        RefreshPortrait();
    }
    public void OnAfinidad2Clicker()
    {
        Player.Instance.Afinidad = Player.Instance.Afinidad == "Afinidad2" ? "Afinidad0" : "Afinidad2";
        UpdateSelectionVisuals();
        RefreshPortrait();
    }
    public void OnAfinidad3Clicker()
    {
        Player.Instance.Afinidad = Player.Instance.Afinidad == "Afinidad3" ? "Afinidad0" : "Afinidad3";
        UpdateSelectionVisuals();
        RefreshPortrait();
    }
}
