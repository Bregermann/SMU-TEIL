using UnityEngine;

public class ClickAndDragCameraManager : MonoBehaviour
{
    // Sensitivity for camera rotation
    public float rotationSensitivity = 1.0f;

    // Store the previous mouse position for delta calculations
    private Vector2 previousMousePosition;

    // Whether the camera is currently being dragged
    private bool isDragging = false;

    // Reference to the main camera
    private Camera mainCamera;

    private void Start()
    {
        // Find the main camera in the scene
        mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("No main camera found in the scene!");
        }
    }

    private void Update()
    {
        // Ensure we have a valid camera before doing anything
        if (mainCamera == null) return;

        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
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

        // If dragging, calculate the rotation
        if (isDragging)
        {
            // Get the current mouse position
            Vector2 currentMousePosition = Input.mousePosition;

            // Calculate the delta in the X and Y axis
            Vector2 mouseDelta = currentMousePosition - previousMousePosition;

            // Apply rotation to the camera based on the mouse delta
            mainCamera.transform.Rotate(0, mouseDelta.x * rotationSensitivity, 0, Space.World);

            // Vertical rotation (pitch) - Negative sign because moving the mouse up should rotate the camera down
            mainCamera.transform.Rotate(-mouseDelta.y * rotationSensitivity, 0, 0, Space.Self);

            // Clamp the pitch rotation to avoid flipping over
            Vector3 currentRotation = mainCamera.transform.localEulerAngles;
            float pitch = currentRotation.x;

            if (pitch > 180)
            {
                pitch -= 360;
            }

            currentRotation.x = Mathf.Clamp(pitch, -90, 90);

            // Apply the clamped rotation
            mainCamera.transform.localEulerAngles = currentRotation;

            // Update the previous mouse position
            previousMousePosition = currentMousePosition;
        }
    }
}
