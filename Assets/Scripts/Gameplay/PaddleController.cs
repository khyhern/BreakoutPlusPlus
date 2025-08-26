using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 12f;
    public float clampX = 8f;

    [Header("Ball Setup")]
    [Tooltip("Ball prefab to spawn.")]
    public BallController ballPrefab;
    public Transform ballSocket;
    public Vector2 ballAttachOffset = new Vector2(0f, 1f);

    InputAction moveAction;
    InputAction launchAction;
    Rigidbody2D rb;

    BallController currentBall;

    float inputX;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        moveAction = InputSystem.actions?.FindAction("Move");
        launchAction = InputSystem.actions?.FindAction("Attack");

        if (moveAction == null || launchAction == null)
            Debug.LogError("Move Action not found. Check Project Settings > Input System Package > Input Actions.");

        if (!ballPrefab)
            Debug.LogError("Ball prefab is not assigned on PaddleController");
    }

    private void OnEnable()
    {
        moveAction?.Enable();
        launchAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        launchAction?.Disable();
    }

    private void Start()
    {
        SpawnBallAttached();
    }

    private void Update()
    {
        inputX = moveAction.ReadValue<Vector2>().x;

        // Launch ball
        if (launchAction.WasPerformedThisFrame())
        {
            if (currentBall && !currentBall.IsLaunched)
            {
                currentBall.Launch(Vector2.up);
            }
        }


        // --- Dev cheat: Respawn ball with R key ---
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            SpawnBallAttached();
            Debug.Log("[DEV] Ball respawned with R key");
        }
    }

    private void FixedUpdate()
    {
        // Move paddle
        float targetX = Mathf.Clamp(rb.position.x + inputX * moveSpeed * Time.fixedDeltaTime, -clampX, clampX);
        Vector2 nextPos = new Vector2(targetX, rb.position.y);
        rb.MovePosition(nextPos);
    }

    public BallController SpawnBallAttached()
    {
        if (currentBall)
        {
            if (!currentBall.IsLaunched)
                return currentBall;
            else
                Destroy(currentBall.gameObject);
        }

        var socket = ballSocket ? ballSocket : transform;
        currentBall = Instantiate(ballPrefab, socket.position, Quaternion.identity);
        currentBall.AttachTo(socket, ballAttachOffset);
        return currentBall;
    }
}
