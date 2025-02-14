using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Path Points")]
    public Transform startPoint;
    public Transform endPoint;
    public Transform cubicControlPoint1;
    public Transform cubicControlPoint2;
    public Transform quadraticControlPoint;

    [Header("Enemy Properties")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private bool useCubicLerp;

    private float progress = 0f; // Tracks movement along the curve

    private void Update()
    {
        MoveAlongPath();
    }

    private void MoveAlongPath()
    {
        progress = Mathf.Clamp01(progress + Time.deltaTime * speed); // Increment and clamp progress

        transform.position = useCubicLerp
            ? CalculateCubicBezier(progress)
            : CalculateQuadraticBezier(progress);

        if (progress >= 1f)
        {
            OnReachEndPoint();
        }
    }

    private Vector3 CalculateQuadraticBezier(float t)
    {
        float oneMinusT = 1 - t;
        return oneMinusT * oneMinusT * startPoint.position +
               2 * oneMinusT * t * quadraticControlPoint.position +
               t * t * endPoint.position;
    }

    private Vector3 CalculateCubicBezier(float t)
    {
        float oneMinusT = 1 - t;
        return oneMinusT * oneMinusT * oneMinusT * startPoint.position +
               3 * oneMinusT * oneMinusT * t * cubicControlPoint1.position +
               3 * oneMinusT * t * t * cubicControlPoint2.position +
               t * t * t * endPoint.position;
    }

    private void OnReachEndPoint()
    {
        GameManager.Instance.TakeDamage(damage);
        Destroy(gameObject);
    }
}
