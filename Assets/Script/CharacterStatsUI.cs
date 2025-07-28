using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviour
{
    public Slider healthSlider;
    public Slider manaSlider;
    public Slider staminaSlider;
    public Image healthFill;
    public Image manaFill;
    public Image staminaFill;
    private CharacterStats characterStats;
    void Start()
    {
        characterStats = FindFirstObjectByType<CharacterStats>();
        if (characterStats != null)
        {
            characterStats.OnHealthChanged += UpdateHealthUI;
            characterStats.OnManaChanged += UpdateManaUI;
            characterStats.OnStaminaChanged += UpdateStaminaUI;
            healthSlider.maxValue = characterStats.maxHealth;
            manaSlider.maxValue = characterStats.maxMana;
            staminaSlider.maxValue = characterStats.maxStamina;
            UpdateHealthUI();
            UpdateManaUI();
            UpdateStaminaUI();
        }
    }
    void UpdateHealthUI()
    {
        healthSlider.value = characterStats.Health;
    }
    void UpdateManaUI()
    {
        manaSlider.value = characterStats.Mana;
    }
    void UpdateStaminaUI()
    {
        staminaSlider.value = characterStats.Stamina;
    }
}
