using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    // Array to store all cameras in the scene
    public Camera[] cameras;

    // Index of the currently active camera
    private int currentCameraIndex = 0;

    // Key to cycle through cameras
    public KeyCode nextCameraKey = KeyCode.RightArrow; // Change as needed
    public KeyCode prevCameraKey = KeyCode.LeftArrow; // Change as needed

    private AudioListener audioListener;

    private void Start()
    {
        audioListener = FindObjectOfType<AudioListener>();
        if (audioListener == null)
        {
            audioListener = new GameObject("Audio Listener").AddComponent<AudioListener>();
        }
        // Ensure only the first camera is active initially
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(i == currentCameraIndex);
            SetActiveCamera(currentCameraIndex);
        }
    }

    private void Update()
    {
        if (cameras.Length <= 1)
        {
            return; // No need to switch if there's only one camera
        }

        // Switch to the next camera
        if (Input.GetKeyDown(nextCameraKey))
        {
            CycleToNextCamera();
        }

        // Switch to the previous camera
        if (Input.GetKeyDown(prevCameraKey))
        {
            CycleToPreviousCamera();
        }
    }

    private void CycleToNextCamera()
    {
        cameras[currentCameraIndex].gameObject.SetActive(false); // Disable current camera
        currentCameraIndex = (currentCameraIndex + 1) % cameras.Length; // Move to the next
       //cameras[currentCameraIndex].gameObject.SetActive(true); // Enable new camera
        SetActiveCamera(currentCameraIndex);
    }

    private void CycleToPreviousCamera()
    {
        cameras[currentCameraIndex].gameObject.SetActive(false); // Disable current camera
        currentCameraIndex = (currentCameraIndex - 1 + cameras.Length) % cameras.Length; // Move to the previous
        //cameras[currentCameraIndex].gameObject.SetActive(true); // Enable new camera
        SetActiveCamera(currentCameraIndex);
    }

    private void SetActiveCamera(int index)
    {
        // Enable the camera at the specified index
        cameras[index].gameObject.SetActive(true);

        // Move the Audio Listener to the active camera
        if (audioListener != null)
        {
            audioListener.transform.SetParent(cameras[index].transform, false);
            audioListener.transform.localPosition = Vector3.zero;
        }
    }
}
