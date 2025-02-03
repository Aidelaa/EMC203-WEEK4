using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProjectileBehavior : MonoBehaviour
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

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if ((transform.position - target.position).magnitude <= destroyDistance)
        {
            GameManager.Instance.AddGold(5);
            Destroy(target.gameObject);
            Destroy(gameObject);
        }
    }
}