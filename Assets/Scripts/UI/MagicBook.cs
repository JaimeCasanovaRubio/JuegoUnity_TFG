using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagicBook : MonoBehaviour
{
    [Header("Armazones")]
    [SerializeField] private Button armazon1;
    [SerializeField] private Button armazon2;
    [SerializeField] private Button armazon3;

    [Header("Afinidades (toggle - puede no haber ninguna)")]
    [SerializeField] private Button afinidad1;
    [SerializeField] private Button afinidad2;
    [SerializeField] private Button afinidad3;

    [Header("Círculos de selección")]
    [SerializeField] private Image circuloArmazon1;
    [SerializeField] private Image circuloArmazon2;
    [SerializeField] private Image circuloArmazon3;
    [SerializeField] private Image circuloAfinidad1;
    [SerializeField] private Image circuloAfinidad2;
    [SerializeField] private Image circuloAfinidad3;

    [Header("Retrato derecho del libro")]
    [SerializeField] private Image portraitImage;

    [Header("Textos descriptivos")]
    [SerializeField] private TextMeshProUGUI textoArmazon;
    [SerializeField] private TextMeshProUGUI textoAfinidad;

    private void OnEnable()
    {
        if (Player.Instance == null) return;

        if (armazon1 != null) armazon1.gameObject.SetActive(Player.Instance.Armazon1);
        if (armazon2 != null) armazon2.gameObject.SetActive(Player.Instance.Armazon2);
        if (armazon3 != null) armazon3.gameObject.SetActive(Player.Instance.Armazon3);

        if (afinidad1 != null) afinidad1.gameObject.SetActive(Player.Instance.Afinidad1);
        if (afinidad2 != null) afinidad2.gameObject.SetActive(Player.Instance.Afinidad2);
        if (afinidad3 != null) afinidad3.gameObject.SetActive(Player.Instance.Afinidad3);

        UpdateSelectionCircles();
        RefreshPortrait();
        RefreshTexts();
    }

    private void UpdateSelectionCircles()
    {
        if (Player.Instance == null) return;

        if (circuloArmazon1 != null) circuloArmazon1.gameObject.SetActive(Player.Instance.Armazon == "Armazon1");
        if (circuloArmazon2 != null) circuloArmazon2.gameObject.SetActive(Player.Instance.Armazon == "Armazon2");
        if (circuloArmazon3 != null) circuloArmazon3.gameObject.SetActive(Player.Instance.Armazon == "Armazon3");

        if (circuloAfinidad1 != null) circuloAfinidad1.gameObject.SetActive(Player.Instance.Afinidad == "Afinidad1");
        if (circuloAfinidad2 != null) circuloAfinidad2.gameObject.SetActive(Player.Instance.Afinidad == "Afinidad2");
        if (circuloAfinidad3 != null) circuloAfinidad3.gameObject.SetActive(Player.Instance.Afinidad == "Afinidad3");
    }

    private void RefreshPortrait()
    {
        if (portraitImage == null || Player.Instance == null) return;
        Sprite sprite = Player.Instance.CurrentDesignSprite;
        if (sprite != null) portraitImage.sprite = sprite;
    }

    private void RefreshTexts()
    {
        if (Player.Instance == null) return;

        if (textoArmazon != null)
        {
            textoArmazon.text = Player.Instance.Armazon switch
            {
                "Armazon1" => "Verdugo de Titanes: cuerpo a cuerpo",
                "Armazon2" => "Garras de Umbra: a distancia",
                "Armazon3" => "El Alambique: proyectil dirigido",
                _ => ""
            };
        }

        if (textoAfinidad != null)
        {
            textoAfinidad.text = Player.Instance.Afinidad switch
            {
                "Afinidad1" => "Pacto Carmesí: curas al atacar",
                "Afinidad2" => "Fiebre ceniza: enemigos arden",
                "Afinidad3" => "Tumba eterna: enemigos aturdidos",
                _ => ""
            };
        }
    }

    public void OnVolverClicker()
    {
        GameManager.Instance.MagicBookCanvas.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnArmazon1Clicker()
    {
        Player.Instance.Armazon = "Armazon1";
        Player.Instance.ActivateDesign();
        UpdateSelectionCircles();
        RefreshPortrait();
        RefreshTexts();
    }
    public void OnArmazon2Clicker()
    {
        Player.Instance.Armazon = "Armazon2";
        Player.Instance.ActivateDesign();
        UpdateSelectionCircles();
        RefreshPortrait();
        RefreshTexts();
    }
    public void OnArmazon3Clicker()
    {
        Player.Instance.Armazon = "Armazon3";
        Player.Instance.ActivateDesign();
        UpdateSelectionCircles();
        RefreshPortrait();
        RefreshTexts();
    }

    public void OnAfinidad1Clicker()
    {
        Player.Instance.Afinidad = Player.Instance.Afinidad == "Afinidad1" ? "Afinidad0" : "Afinidad1";
        Player.Instance.ActivateDesign();
        UpdateSelectionCircles();
        RefreshPortrait();
        RefreshTexts();
    }
    public void OnAfinidad2Clicker()
    {
        Player.Instance.Afinidad = Player.Instance.Afinidad == "Afinidad2" ? "Afinidad0" : "Afinidad2";
        Player.Instance.ActivateDesign();
        UpdateSelectionCircles();
        RefreshPortrait();
        RefreshTexts();
    }
    public void OnAfinidad3Clicker()
    {
        Player.Instance.Afinidad = Player.Instance.Afinidad == "Afinidad3" ? "Afinidad0" : "Afinidad3";
        Player.Instance.ActivateDesign();
        UpdateSelectionCircles();
        RefreshPortrait();
        RefreshTexts();
    }
}
