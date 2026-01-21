using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Pan")]
    public float panSpeed = 20f;
    public float dragSpeed = 0.02f;

    [Header("Bounds (optional)")]
    public Vector2 minBounds = new(-100, -100);
    public Vector2 maxBounds = new(100, 100);

    Vector3 lastMousePos;

    void Update()
    {
        HandleKeyboardPan();
        ClampPosition();
    }

    void HandleKeyboardPan()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (h == 0 && v == 0) return;

        Vector3 right = transform.right;
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;

        Vector3 move = (right * h + forward * v).normalized;
        transform.position += move * panSpeed * Time.deltaTime;
    }

    void ClampPosition()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minBounds.x, maxBounds.x);
        pos.z = Mathf.Clamp(pos.z, minBounds.y, maxBounds.y);
        transform.position = pos;
    }
}