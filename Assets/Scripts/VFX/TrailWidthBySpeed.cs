using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class TrailWidthBySpeed : MonoBehaviour
{
    public Rigidbody2D ballRb;
    public float minWidth = 0.10f;
    public float maxWidth = 0.22f;
    public float maxSpeedForWidth = 14f;

    TrailRenderer tr;

    private void Awake()
    {
        tr = GetComponent<TrailRenderer>();
    }

    private void LateUpdate()
    {
        if (!ballRb) return;
        float t = Mathf.Clamp01(ballRb.linearVelocity.magnitude / maxSpeedForWidth);
        tr.widthMultiplier = Mathf.Lerp(minWidth, maxWidth, t);
    }
}
