using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float destroyDistance = 0.5f;
    private Transform target;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Move towards target
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Rotate towards target smoothly
        Vector3 direction = target.position - transform.position;
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, direction), Time.deltaTime * speed);
        }

        // Destroy target and missile if within range
        if (Vector3.Distance(transform.position, target.position) <= destroyDistance)
        {
            Destroy(target.gameObject);
            Destroy(gameObject);
        }
    }
}
