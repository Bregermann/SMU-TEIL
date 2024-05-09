using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    // Assign these in the Unity Inspector with the objects you'd like to switch between
    public Transform[] targetTransforms;

    // Option to control the speed of the camera movement
    public float moveSpeed = 2.0f;

    // The current target index
    private int currentTargetIndex = 0;

    // Camera reference
    private Camera mainCamera;

    private void Start()
    {
        // Get the main camera reference
        mainCamera = Camera.main;

        // Set the initial position of the camera
        if (targetTransforms.Length > 0)
        {
            mainCamera.transform.position = targetTransforms[currentTargetIndex].position;
        }
    }

    private void Update()
    {
        // Check for input to cycle through targets
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CycleToNextTarget();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CycleToPreviousTarget();
        }

        // Smoothly move the camera to the current target's position
        if (targetTransforms.Length > 0)
        {
            Vector3 targetPosition = targetTransforms[currentTargetIndex].position;
            mainCamera.transform.position = Vector3.Lerp(
                mainCamera.transform.position,
                targetPosition,
                Time.deltaTime * moveSpeed
            );
        }
    }

    // Switch to the next target
    public void CycleToNextTarget()
    {
        if (targetTransforms.Length > 1)
        {
            currentTargetIndex = (currentTargetIndex + 1) % targetTransforms.Length;
        }
    }

    // Switch to the previous target
    public void CycleToPreviousTarget()
    {
        if (targetTransforms.Length > 1)
        {
            currentTargetIndex = (currentTargetIndex - 1 + targetTransforms.Length) % targetTransforms.Length;
        }
    }
}
