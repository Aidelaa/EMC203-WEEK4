using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; 

public class TurretBehavior : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] public float fireRange = 10f;  // This will be upgraded
    [SerializeField] public float fireCooldown = 2f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject homingMissilePrefab;  // Reference to homing missile prefab
    private float lastFireTime = 0f;
    public bool canShootHomingMissile = false; // If the turret can shoot homing missiles
    public float bulletKillDistance = 5f;  // Distance where the bullet can kill enemies




    // For upgrades:
    public void UpgradeFireRate(float newCooldown)
    {
        fireCooldown = newCooldown;
    }

    public void UpgradeFireRange(float newRange)
    {
        fireRange = newRange;
    }

    private void Update()
    {
        GameObject target = FindClosestEnemy();
        if (target == null) return;
        
        RotateTowards(target.transform);
        
        if (Time.time >= lastFireTime + fireCooldown)
        {
            FireAt(target.transform);
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDist = fireRange;
        foreach (GameObject enemy in enemies)
        {
            float dist = (transform.position - enemy.transform.position).magnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }
        return closest;
    }

    private void RotateTowards(Transform target)
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FireAt(Transform target)
    {
        if (GameManager.Instance.homingMissileUnlocked)  // Only fire homing missiles if unlocked
        {
            GameObject homingMissile = Instantiate(homingMissilePrefab, firePoint.position, firePoint.rotation);
            homingMissile.GetComponent<HomingMissile>().SetTarget(target);
        }
        else
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
            projectile.GetComponent<ProjectileBehavior>().SetTarget(target);
        }
        lastFireTime = Time.time;
    }

    public void SetFireCooldown(float newCooldown)
{
    fireCooldown = newCooldown;
}

public void SetFireRange(float newRange)
{
    fireRange = newRange;
}

}
