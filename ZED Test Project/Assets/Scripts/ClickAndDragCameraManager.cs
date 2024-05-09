using UnityEngine;

public class ClickAndDragCameraManager : MonoBehaviour
{
    // Sensitivity for camera rotation
    public float rotationSensitivity = 1.0f;

    // Store the previous mouse position for delta calculations
    private Vector2 previousMousePosition;

    // Whether the camera is currently being dragged
    private bool isDragging = false;

    // Reference to all cameras in the scene
    private Camera[] allCameras;

    private void Start()
    {
        // Find all cameras in the scene
        allCameras = Camera.allCameras;

        if (allCameras.Length == 0)
        {
            Debug.LogError("No cameras found in the scene!");
        }
    }

    private void Update()
    {
        if (allCameras.Length == 0) return; // No cameras to rotate

        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Start dragging and capture the initial mouse position
            isDragging = true;
            previousMousePosition = Input.mousePosition;
        }

        // If the mouse button is released, stop dragging
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // If dragging, calculate the rotation for all cameras
        if (isDragging)
        {
            // Get the current mouse position
            Vector2 currentMousePosition = Input.mousePosition;

            // Calculate the delta in the X and Y axis
            Vector2 mouseDelta = currentMousePosition - previousMousePosition;

            foreach (var camera in allCameras)
            {
                if (camera == null) continue; // Skip null cameras

                // Apply horizontal (yaw) rotation around the Y-axis
                camera.transform.Rotate(0, mouseDelta.x * rotationSensitivity, 0, Space.World);

                // Apply vertical (pitch) rotation around the X-axis
                camera.transform.Rotate(-mouseDelta.y * rotationSensitivity, 0, 0, Space.Self);

                // Clamp pitch rotation to avoid flipping over
                Vector3 currentRotation = camera.transform.localEulerAngles;

                float pitch = currentRotation.x;
                if (pitch > 180)
                {
                    pitch -= 360;
                }

                currentRotation.x = Mathf.Clamp(pitch, -90, 90);

                camera.transform.localEulerAngles = currentRotation;
            }

            // Update the previous mouse position
            previousMousePosition = currentMousePosition;
        }
    }
}
