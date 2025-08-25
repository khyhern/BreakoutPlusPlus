using UnityEngine;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    public float moveSpeed = 12f;
    public float clampX = 8f;
    InputAction moveAction;

    private void Awake()
    {
        moveAction = InputSystem.actions?.FindAction("Move");
        if (moveAction == null)
            Debug.LogError("Move Action not found. Check Project Settings > Input System Package > Input Actions.");
    }

    private void OnEnable()
    {
        moveAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
    }

    private void Update()
    {
        float inputX = moveAction.ReadValue<Vector2>().x;

        var p = transform.position;
        p.x = Mathf.Clamp(p.x + inputX * moveSpeed * Time.deltaTime, -clampX, clampX);
        transform.position = p;
    }

}
