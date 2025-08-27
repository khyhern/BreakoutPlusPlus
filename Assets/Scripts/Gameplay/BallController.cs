using UnityEngine;


[RequireComponent (typeof(Rigidbody2D))]
public class BallController : MonoBehaviour
{
    [Header("Speed")]
    public float launchSpeed = 500f;
    // public float minSpeed = 40f, maxSpeed = 60f;

    [Header("Attach Follow")]
    public Transform followTarget;
    public Vector2 localOffset;

    public bool IsLaunched { get; private set; }

    Rigidbody2D rb;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        if (!IsLaunched && followTarget)
        {
            Vector2 target = (Vector2)followTarget.position + localOffset;
            rb.MovePosition(target);
        }

        if (IsLaunched)
        {
            float speed = rb.linearVelocity.magnitude;
            //if (speed < minSpeed && speed > 0f)
            //    rb.linearVelocity = rb.linearVelocity.normalized * minSpeed;
            //else if (speed > maxSpeed)
            //    rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

            Debug.Log(speed);
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

    public void Launch()
    {
        if (IsLaunched) return;
        IsLaunched = true;
        followTarget = null;

        rb.bodyType = RigidbodyType2D.Dynamic;

        Vector2 force = Vector2.zero;
        force.x = Random.Range(-1f, 1f);
        force.y = 1f;

        rb.AddForce(force.normalized * this.launchSpeed);
    }

}
