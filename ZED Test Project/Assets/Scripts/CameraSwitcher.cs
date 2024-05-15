using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CameraSwitcher : MonoBehaviour
{
    // List of cameras to manage
    private List<Camera> cameras = new List<Camera>();

    // Index of the currently active camera
    private int currentCameraIndex = 0;

    // Keys to cycle through cameras
    public KeyCode nextCameraKey = KeyCode.RightArrow;
    public KeyCode prevCameraKey = KeyCode.LeftArrow;

    // Tag used to find cameras
    public string cameraTag = "SwitchableCamera"; // Default tag for switchable cameras

    // Reference to the Audio Listener
    private AudioListener audioListener;

    private void Start()
    {
        // Find and add cameras with the specified tag
        FindAndAddCameras();

        // Ensure there's at least one camera
        if (cameras.Count == 0)
        {
            Debug.LogError("No cameras found with the specified tag!");
            return;
        }

        // Find or create the Audio Listener
        audioListener = FindObjectOfType<AudioListener>();
        if (audioListener == null)
        {
            audioListener = new GameObject("AudioListener").AddComponent<AudioListener>();
        }

        // Activate the first camera and move the Audio Listener
        SetActiveCamera(currentCameraIndex);
    }

    private void Update()
    {
        if (cameras.Count <= 1)
        {
            return; // No need to switch if there's only one camera
        }

        // Check for input to switch cameras
        if (Input.GetKeyDown(nextCameraKey))
        {
            CycleToNextCamera();
        }
        else if (Input.GetKeyDown(prevCameraKey))
        {
            CycleToPreviousCamera();
        }
    }

    private void CycleToNextCamera()
    {
        // Find and add new cameras before switching
        FindAndAddCameras();

        // Disable the current camera if it's not destroyed
        if (cameras[currentCameraIndex] != null)
        {
            cameras[currentCameraIndex].gameObject.SetActive(false);
        }

        // Move to the next available camera
        do
        {
            currentCameraIndex = (currentCameraIndex + 1) % cameras.Count;
        } while (cameras[currentCameraIndex] == null);

        // Activate the new camera and move the Audio Listener
        SetActiveCamera(currentCameraIndex);
    }

    private void CycleToPreviousCamera()
    {
        // Find and add new cameras before switching
        FindAndAddCameras();

        // Disable the current camera if it's not destroyed
        if (cameras[currentCameraIndex] != null)
        {
            cameras[currentCameraIndex].gameObject.SetActive(false);
        }

        // Move to the previous available camera
        do
        {
            currentCameraIndex = (currentCameraIndex - 1 + cameras.Count) % cameras.Count;
        } while (cameras[currentCameraIndex] == null);

        // Activate the new camera and move the Audio Listener
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

    private void FindAndAddCameras()
    {
        // Find all cameras with the specified tag
        Camera[] foundCameras = GameObject.FindGameObjectsWithTag(cameraTag)
            .Select(go => go.GetComponent<Camera>())
            .Where(cam => cam != null)
            .OrderBy(cam => cam.name) // Sort cameras alphabetically based on name
            .ToArray();

        // Add new cameras to the list if they're not already included
        foreach (Camera cam in foundCameras)
        {
            if (!cameras.Contains(cam))
            {
                cameras.Add(cam);
            }
        }

        // Remove destroyed cameras from the list
        cameras.RemoveAll(cam => cam == null);
    }
    // Method to get the list of cameras
    public List<Camera> GetCameras()
    {
        return cameras;
    }

    // Method to get the current active camera index
    public int GetCurrentCameraIndex()
    {
        return currentCameraIndex;
    }
}
