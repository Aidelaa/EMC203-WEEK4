using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int playerHP = 10;
    public int gold = 0;
    public bool homingMissileUnlocked = false;  // To track if the homing missile is unlocked
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI goldText;
    public GameObject failUI;
    public TowerUpgradeSystem towerUpgradeSystem;  // Reference to the TowerUpgradeSystem script

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        playerHP -= amount;
        UpdateUI();
        if (playerHP <= 0)
        {
            // Show fail UI for 1 second and then hide it
            ShowFailUI();
        }
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        hpText.text = "HP: " + GameManager.Instance.playerHP;  
        goldText.text = "Gold: " + gold;

        // Update UI in TowerUpgradeSystem to reflect button availability
        if (towerUpgradeSystem != null)
        {
            towerUpgradeSystem.UpdateUI();  // Call the method from TowerUpgradeSystem to update buttons
        }
    }

    private void ShowFailUI()
    {
        failUI.SetActive(true);
        Invoke("HideFailUI", 1f);  // Hide after 1 second
    }

    private void HideFailUI()
    {
        failUI.SetActive(false);
    }
}
