using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BedInteraction : MonoBehaviour
{
    public GameObject sleepMenuUI;
    public Button restButton;
    public Button sleepButton;
    [SerializeField] private List<Sprite> visualSprites;
    private SpriteRenderer spriteRenderer; 
    private CharacterStats characterStats;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (visualSprites != null && visualSprites.Count > 0)
        {
            spriteRenderer.sprite = visualSprites[0];
        }
        sleepMenuUI.SetActive(false);
        restButton.onClick.AddListener(Rest);
        sleepButton.onClick.AddListener(SleepTillNextDay);
        if (Player.Instance != null)
        {
            characterStats = Player.Instance.GetCharacterStats();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (visualSprites != null && visualSprites.Count > 0)
        {
            spriteRenderer.sprite = visualSprites[1];
        }
        Debug.Log("Trigger Enter: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered bed area");
            Player.Instance?.SetSleeping(true);
            OpenSleepMenu();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (visualSprites != null && visualSprites.Count > 0)
        {
            spriteRenderer.sprite = visualSprites[0];
        }
        Player.Instance.SetSleeping(false);
        if (other.CompareTag("Player"))
        {
            StopRest();
        }
    }
    private void OpenSleepMenu()
    {
        sleepMenuUI.SetActive(true);
        Time.timeScale = 0;
    }
    private void Rest()
    {
        Debug.Log("Người chơi bắt đầu nghỉ ngơi...");
        sleepMenuUI.SetActive(false);
        Time.timeScale = 1;
        if (characterStats != null)
        {
            characterStats.StartResting();
        }
    }
    private void StopRest()
    {
        Debug.Log("Người chơi kết thúc nghỉ ngơi...");
        if (characterStats != null)
        {
            characterStats.StopResting();
        }
    }
    private void SleepTillNextDay()
    {
        Debug.Log("Ngủ đến ngày mới...");
        GameTimeManager.Instance.ForceNewDay();
        sleepMenuUI.SetActive(false);
        Time.timeScale = 1;
        if (characterStats != null)
        {
            characterStats.RegenerateHealth(characterStats.maxHealth);
            characterStats.RegenerateMana(characterStats.maxMana);
            characterStats.RegenerateStamina(characterStats.maxStamina);
        }
    }
}
