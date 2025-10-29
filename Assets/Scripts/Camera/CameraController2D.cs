using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController2D : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private LevelBound2D bounds;

    private Camera cam;
    private Vector3 targetPos;

    void Awake()
    {
        cam = GetComponent<Camera>();
        targetPos = transform.position;
    }

    private void OnEnable()
    {
        targetPos = transform.position;
    }

    public void SnapToCurrent(Vector3 pos)
    {
        targetPos = pos;
        transform.position = pos;
    }

    void Update()
    {
        Vector2 input = new(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical")    
        );

        if (bounds != null)
        {
            if (!bounds.allowHorizontal) input.x = 0f;
            if (!bounds.allowVertical) input.y = 0f;
        }

        if (input.sqrMagnitude > 1f) input.Normalize();

        if (input != Vector2.zero)
            targetPos += (Vector3)(input * moveSpeed * Time.deltaTime);

        ClampToMinMax(ref targetPos);
        transform.position = targetPos;
    }

    private void ClampToMinMax(ref Vector3 pos)
    {
        if (bounds == null) return;

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        float minX = bounds.minX + halfW;
        float maxX = bounds.maxX - halfW;
        float minY = bounds.minY + halfH;
        float maxY = bounds.maxY - halfH;

        // If camera view is larger than bounds on an axis, lock to center of that axis
        if (maxX < minX) pos.x = (bounds.minX + bounds.maxX) * 0.5f;
        else pos.x = Mathf.Clamp(pos.x, minX, maxX);

        if (maxY < minY) pos.y = (bounds.minY + bounds.maxY) * 0.5f;
        else pos.y = Mathf.Clamp(pos.y, minY, maxY);

        pos.z = transform.position.z; // keep camera Z
    }
}
