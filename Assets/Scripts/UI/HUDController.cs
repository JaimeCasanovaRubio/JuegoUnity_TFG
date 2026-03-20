using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Ability")]
    [SerializeField] private Slider abilitySlider;
    [SerializeField] private TextMeshProUGUI abilityText;

    private Entity playerEntity;
    private Player player;

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (player != null)
        {
            UpdateHealthUI();
            UpdateAbilityUI();
        }
        else
        {
            FindPlayer();
        }
    }

    private void FindPlayer()
    {
        player = FindObjectOfType<Player>();
        if (player != null)
        {
            playerEntity = player as Entity;
        }
    }

    private void UpdateHealthUI()
    {
        if (playerEntity != null && healthSlider != null)
        {
            float current = playerEntity.CurrentHealth;
            float max = playerEntity.MaxHealth;

            healthSlider.maxValue = max;
            healthSlider.value = current;

            if (healthText != null)
            {
                healthText.text = $"{Mathf.CeilToInt(current)}/{Mathf.CeilToInt(max)}";
            }
        }
    }

    private void UpdateAbilityUI()
    {
        if (player != null && abilitySlider != null)
        {
            float remaining = player.AbilityCooldownRemaining;
            float total = player.AbilityCooldownTotal;

            if (total > 0)
            {
                abilitySlider.value = 1f-(remaining / total);

                if (abilityText != null)
                {
                    abilityText.text = remaining > 0 ? $"{remaining:F1}s" : "Ready";
                }
            }
            else
            {
                abilitySlider.value = 1f;
                if (abilityText != null)
                {
                    abilityText.text = "Ready";
                }
            }
        }
    }
}
