using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    [SerializeField] private float turnSpeed = 5f;
    [SerializeField] private float attackRadius = 10f;
    [SerializeField] private float reloadTime = 2f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject seekerMissilePrefab;

    private float previousShotTime = 0f;
    private bool canShootHomingMissile = false;
    private float bulletKillDistance = 5f;

    private void Update()
    {
        GameObject target = FindClosestEnemy();
        if (target == null) return;

        RotateTowards(target.transform);

        if (Time.time >= previousShotTime + reloadTime)
        {
            FireAt(target.transform);
        }
    }

    private GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null; 
        float minDist = attackRadius;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);
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
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    private void FireAt(Transform target)
    {
        GameObject projectile;

        if (GameManager.Instance.homingMissileUnlocked)
        {
            projectile = Instantiate(seekerMissilePrefab, shootPoint.position, shootPoint.rotation);
            projectile.GetComponent<HomingMissile>().SetTarget(target);
        }
        else
        {
            projectile = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            projectile.GetComponent<ProjectileBehavior>().SetTarget(target);
        }

        previousShotTime = Time.time;
    }

    // Upgrade Methods
    public void SetFireCooldown(float newCooldown) => reloadTime = newCooldown;
    public void SetFireRange(float newRange) => attackRadius = newRange;
}
