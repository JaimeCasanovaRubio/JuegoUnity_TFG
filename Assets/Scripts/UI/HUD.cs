using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// HUD del juego que muestra vida y cooldown de habilidad.
/// Equivalente a la lógica de drawPlayerHealth() y drawAbilityCooldown() de GameScreenModel.java.
/// </summary>
public class HUD : MonoBehaviour
{
    [Header("Health Display")]
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Color healthFullColor = Color.green;
    [SerializeField] private Color healthLowColor = Color.red;

    [Header("Ability Cooldown")]
    [SerializeField] private Image cooldownBarFill;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private Color cooldownReadyColor = Color.cyan;
    [SerializeField] private Color cooldownChargingColor = Color.gray;

    [Header("Player Reference")]
    [SerializeField] private Player playerReference;

    private void Start()
    {
        // Buscar al jugador si no está asignado
        if (playerReference == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerReference = playerObj.GetComponent<Player>();
            }
        }
    }

    private void Update()
    {
        if (playerReference != null)
        {
            UpdateHealthDisplay();
            UpdateCooldownDisplay();
        }
    }

    /// <summary>
    /// Actualiza la visualización de la barra de vida.
    /// </summary>
    private void UpdateHealthDisplay()
    {
        float healthPercent = (float)playerReference.Health / playerReference.MaxHealth;

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = healthPercent;
            // Interpolar color según la vida
            healthBarFill.color = Color.Lerp(healthLowColor, healthFullColor, healthPercent);
        }

        if (healthText != null)
        {
            healthText.text = $"{playerReference.Health}/{playerReference.MaxHealth}";
        }
    }

    /// <summary>
    /// Actualiza la visualización del cooldown de habilidad.
    /// </summary>
    private void UpdateCooldownDisplay()
    {
        float cooldownPercent = 1f - (playerReference.AbilityCooldown / playerReference.MaxAbilityCooldown);

        if (cooldownBarFill != null)
        {
            cooldownBarFill.fillAmount = cooldownPercent;
            // Color diferente si está listo o cargando
            cooldownBarFill.color = playerReference.CanUseAbility ? cooldownReadyColor : cooldownChargingColor;
        }

        if (cooldownText != null)
        {
            if (playerReference.CanUseAbility)
            {
                cooldownText.text = "READY";
            }
            else
            {
                cooldownText.text = $"{playerReference.AbilityCooldown:F1}s";
            }
        }
    }

    /// <summary>
    /// Establece la referencia al jugador.
    /// </summary>
    public void SetPlayer(Player player)
    {
        playerReference = player;
    }
}
