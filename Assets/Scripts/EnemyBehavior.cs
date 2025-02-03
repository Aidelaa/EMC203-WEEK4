using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; 


public class EnemyBehavior : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float speed = 2f;
    public bool useCubicLerp;
    private float t = 0f;
    public int damage = 1;  

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Assuming the turret is tagged as "Turret"
        if (other.CompareTag("Turret"))
        {
            // Call TakeDamage method from GameManager to reduce HP
            GameManager.Instance.TakeDamage(damage);
            Destroy(gameObject);  // Destroy the enemy after it hits the turret
        }
    }

    private void Update()
    {
        t += Time.deltaTime * speed;
        if (useCubicLerp)
        {
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t * t * (3f - 2f * t));
        }
        else
        {
            transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t * t);
        }

        if (t >= 1f)
        {
            GameManager.Instance.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
