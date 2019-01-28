using UnityEngine.UI;
using UnityEngine;
using System;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField] Text healthText;
    [SerializeField] Text speedText;
    [SerializeField] Text damageText;
    [SerializeField] Text fireRateText;

    [SerializeField] Button healthUpgradeButton;
    [SerializeField] Button speedUpgradeButton;
    [SerializeField] Button damageUpgradeButton;
    [SerializeField] Button fireRateUpgradeButton;

    [SerializeField] float healthMultiplier = 1.2f;
    [SerializeField] int upgradeCost = 50;

    int healthUpgradesCount = 0;
    int speedUpgradesCount = 0;
    int damageUpgradesCount = 0;
    int fireRateUpgradesCount = 0;

    private PlayerStats stats;
    
    void UpdateValues()
    {
        healthText.text = "Health: " + stats.maxHealth;
        speedText.text = "Speed: " + stats.movementSpeed;
        damageText.text = "Damage MF: " + stats.weaponDamageMultiplier;
        fireRateText.text = "Fire Rate MF: " + stats.weaponFireRateMultiplier;
    }

    private void OnEnable()
    {
        stats = PlayerStats.instance;
        UpdateValues();
        ValidateUpgradeButtons();
        var colorsBlock = healthUpgradeButton.colors;
        colorsBlock.disabledColor = Color.gray;
        healthUpgradeButton.colors = colorsBlock;
    }

    public void UpgradeHealth()
    {
        healthUpgradesCount++;
        GameMaster.Money -= healthUpgradesCount * upgradeCost;
        ValidateUpgradeButtons();
        stats.maxHealth = Mathf.RoundToInt(stats.maxHealth * healthMultiplier);
        UpdateValues();
        AudioManager.instance.PlaySound("Money");
    }

    public void UpgradeSpeed()
    {
        speedUpgradesCount++;
        GameMaster.Money -= speedUpgradesCount * upgradeCost;
        ValidateUpgradeButtons();
        stats.movementSpeed += 0.5f;
        UpdateValues();
        AudioManager.instance.PlaySound("Money");
    }

    public void UpgradeFireDamage()
    {
        damageUpgradesCount++;
        GameMaster.Money -= damageUpgradesCount * upgradeCost;
        ValidateUpgradeButtons();
        stats.weaponDamageMultiplier += 0.1f;
        UpdateValues();
        AudioManager.instance.PlaySound("Money");
    }

    public void UpgradeFireRate()
    {
        fireRateUpgradesCount++;
        GameMaster.Money -= fireRateUpgradesCount * upgradeCost;
        ValidateUpgradeButtons();
        stats.weaponFireRateMultiplier = (float)Math.Round(stats.weaponFireRateMultiplier + 0.2f, 2);
        UpdateValues();
        AudioManager.instance.PlaySound("Money");
    }

    private void ValidateUpgradeButtons()
    {
        healthUpgradeButton.enabled = healthUpgradeButton.interactable = GameMaster.Money >= (healthUpgradesCount + 1) * upgradeCost;
        speedUpgradeButton.enabled = GameMaster.Money >= (speedUpgradesCount + 1) * upgradeCost;
        damageUpgradeButton.enabled = GameMaster.Money >= (damageUpgradesCount + 1) * upgradeCost;
        fireRateUpgradeButton.enabled = GameMaster.Money >= (fireRateUpgradesCount + 1) * upgradeCost;

        healthUpgradeButton.GetComponentInChildren<Text>().text = "UPGRADE (" + (healthUpgradesCount + 1) * upgradeCost + ")";
        speedUpgradeButton.GetComponentInChildren<Text>().text = "UPGRADE (" + (speedUpgradesCount + 1) * upgradeCost + ")";
        damageUpgradeButton.GetComponentInChildren<Text>().text = "UPGRADE (" + (damageUpgradesCount + 1) * upgradeCost + ")";
        fireRateUpgradeButton.GetComponentInChildren<Text>().text = "UPGRADE (" + (fireRateUpgradesCount + 1) * upgradeCost + ")";
    }
}
