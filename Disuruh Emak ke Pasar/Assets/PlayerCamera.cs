using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;  // The object to orbit around
    public float distance = 5.0f;
    public float sensitivity = 2.0f;
    public float minYAngle = -20f, maxYAngle = 80f;
    public float zoomSpeed = 2.0f;
    public float minZoom = 2.0f, maxZoom = 10.0f;

    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Start()
    {
        if (target != null)
        {
            Vector3 angles = transform.eulerAngles;
            rotationX = angles.y;
            rotationY = angles.x;
        }
    }

    void Update()
    {
        if (target == null) return;

        // Rotate with right mouse button
        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivity;
            rotationY -= Input.GetAxis("Mouse Y") * sensitivity;
            rotationY = Mathf.Clamp(rotationY, minYAngle, maxYAngle);
        }

        // Zoom with scroll wheel
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        distance = Mathf.Clamp(distance, minZoom, maxZoom);
    }

    void LateUpdate()
    {
        if (target == null) return;

        Quaternion rotation = Quaternion.Euler(rotationY, rotationX, 0);
        Vector3 position = target.position - (rotation * Vector3.forward * distance);

        transform.rotation = rotation;
        transform.position = position;
    }
}
