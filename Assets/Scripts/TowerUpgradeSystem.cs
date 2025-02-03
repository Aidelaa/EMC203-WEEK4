using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerUpgradeSystem : MonoBehaviour
{
    public Button speedUpgradeButton;
    public Button rangeUpgradeButton;
    public Button bulletKillDistanceUpgradeButton;
    public Button homingMissileUpgradeButton;
    public TextMeshProUGUI goldText;

    // Reference to TurretBehavior
    public TurretBehavior turretBehavior;  // Drag the Turret in the inspector

    private void Start()
    {
        speedUpgradeButton.onClick.AddListener(UpgradeSpeed);
        rangeUpgradeButton.onClick.AddListener(UpgradeRange);
        bulletKillDistanceUpgradeButton.onClick.AddListener(UpgradeBulletKillDistance);
        homingMissileUpgradeButton.onClick.AddListener(UnlockHomingMissile);

        UpdateUI();
    }

    public void UpdateUI()
    {
        goldText.text = "Gold: " + GameManager.Instance.gold;

        // Update button availability based on gold
        speedUpgradeButton.interactable = GameManager.Instance.gold >= 5;
        rangeUpgradeButton.interactable = GameManager.Instance.gold >= 10;
        bulletKillDistanceUpgradeButton.interactable = GameManager.Instance.gold >= 15;
        homingMissileUpgradeButton.interactable = GameManager.Instance.gold >= 20 && !GameManager.Instance.homingMissileUnlocked;
    }

    private void UpgradeSpeed()
    {
        if (GameManager.Instance.gold >= 5)
        {
            GameManager.Instance.AddGold(-5);

            // Decrease the fire rate (make the turret fire faster)
            if (turretBehavior != null)
            {
                turretBehavior.SetFireCooldown(turretBehavior.fireCooldown - 0.2f);  // Decrease cooldown by 0.2s
                Debug.Log("Speed upgraded!");
            }
        }
        UpdateUI();
    }

    private void UpgradeRange()
    {
        if (GameManager.Instance.gold >= 10)
        {
            GameManager.Instance.AddGold(-10);

            // Increase the fire range
            if (turretBehavior != null)
            {
                turretBehavior.SetFireRange(turretBehavior.fireRange + 2f);  // Increase range by 2 units
                Debug.Log("Range upgraded!");
            }
        }
        UpdateUI();
    }

private void UpgradeBulletKillDistance()
{
    if (GameManager.Instance.gold >= 15)
    {
        GameManager.Instance.AddGold(-15);

        // Increase the bullet kill distance for the turret
        if (turretBehavior != null)
        {
            turretBehavior.bulletKillDistance += 2f;  // Increase by 2 units
            Debug.Log("Bullet kill distance upgraded!");
        }
    }
    UpdateUI();
}

    private void UnlockHomingMissile()
    {
        if (GameManager.Instance.gold >= 20 && !GameManager.Instance.homingMissileUnlocked)
        {
            GameManager.Instance.AddGold(-20);
            GameManager.Instance.homingMissileUnlocked = true;
            Debug.Log("Homing missile unlocked!");

            // Unlock homing missile by changing the turret's bullet type or enabling a new missile prefab
            if (turretBehavior != null)
            {
                turretBehavior.canShootHomingMissile = true;  // Assuming you have this flag in your turret script
            }
        }
        UpdateUI();
    }
}
