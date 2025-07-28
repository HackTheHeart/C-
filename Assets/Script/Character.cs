using UnityEngine;
using System;
using System.Collections;

public class CharacterStats : MonoBehaviour
{
    public event Action OnHealthChanged;
    public event Action OnManaChanged;
    public event Action OnStaminaChanged;
    public int maxHealth = 150;
    public int maxMana = 100;
    public int maxStamina = 250;
    private int currentHealth;
    private int currentMana;
    private int currentStamina;
    private PlayerAnimation playerAnimation;
    private bool isResting = false;
    void Start()
    {
        currentHealth = maxHealth;
        currentMana = maxMana;
        currentStamina = maxStamina;
        OnHealthChanged?.Invoke();
        OnManaChanged?.Invoke();
        OnStaminaChanged?.Invoke();
        if (Player.Instance != null)
        {
            playerAnimation = Player.Instance.GetComponent<PlayerAnimation>();
        }
    }
    public int Health => currentHealth;
    public int Mana => currentMana;
    public int Stamina => currentStamina;
    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, maxHealth);
        OnHealthChanged?.Invoke();
        if (currentHealth > 0)
        {
            playerAnimation?.PlayDamageAnimation();
        }
        else
        {
            playerAnimation?.PlayDeathAnimation();
        }
    }
    public void UseMana(int amount)
    {
        if (currentMana <= 0) return; 
        currentMana = Mathf.Clamp(currentMana - amount, 0, maxMana);
        OnManaChanged?.Invoke();
    }
    public void UseStamina(int amount)
    {
        if (currentStamina <= 0) return;
        currentStamina = Mathf.Clamp(currentStamina - amount, 0, maxStamina);
        OnStaminaChanged?.Invoke();
    }
    public void RegenerateHealth(int amount)
    {
        if (currentHealth >= maxHealth) return;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke();
    }
    public void RegenerateMana(int amount)
    {
        if (currentMana >= maxMana) return;
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        OnManaChanged?.Invoke();
    }
    public void RegenerateStamina(int amount)
    {
        if (currentStamina >= maxStamina) return; 
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, maxStamina);
        OnStaminaChanged?.Invoke();
    }
    private IEnumerator RegenerateStatsOverTime()
    {
        while (isResting)
        {
            RegenerateHealth(2);  
            RegenerateMana(3);     
            RegenerateStamina(5);  
            yield return new WaitForSeconds(1f);
        }
    }
    public void StartResting()
    {
        if (!isResting)
        {
            isResting = true;
            StartCoroutine(RegenerateStatsOverTime());
        }
    }
    public void StopResting()
    {
        isResting = false;
    }
}
