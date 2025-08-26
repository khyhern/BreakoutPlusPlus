using UnityEngine;


[RequireComponent (typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Header("Speed")]
    public float launchSpeed = 8f;
    public float minSpeed = 6f, maxSpeed = 12f;

    [Header("Attach Follow")]
    public Transform followTarget;
    public Vector2 localOffset;

    public bool IsLaunched { get; private set; }

    Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        if (!IsLaunched && followTarget)
        {
            transform.position = (Vector2)followTarget.position + localOffset;
        }
    }

    public void AttachTo(Transform target, Vector2 offset)
    {
        IsLaunched = false;
        followTarget = target;
        localOffset = offset;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public void Launch(Vector2 dir)
    {
        if (IsLaunched) return;
        IsLaunched = true;
        followTarget = null;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = dir.normalized * launchSpeed;
    }

}
