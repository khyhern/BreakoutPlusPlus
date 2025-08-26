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

    BallController currentBall;



    private void Awake()
    {
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
        float inputX = moveAction.ReadValue<Vector2>().x;

        // Move paddle
        var p = transform.position;
        p.x = Mathf.Clamp(p.x + inputX * moveSpeed * Time.deltaTime, -clampX, clampX);
        transform.position = p;

        // Launch ball
        if (launchAction.WasPerformedThisFrame())
        {
            if (currentBall && !currentBall.IsLaunched)
            {
                currentBall.Launch(Vector2.up);
            }
        }
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
