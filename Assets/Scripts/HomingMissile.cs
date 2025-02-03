using UnityEngine;
using UnityEngine.UI;  
using TMPro; 

public class HomingMissile : MonoBehaviour
{
    public float speed = 10f;
    public float destroyDistance = 0.5f;
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

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if ((transform.position - target.position).magnitude <= destroyDistance)
        {
            Destroy(target.gameObject);
            Destroy(gameObject);
        }
    }
}
