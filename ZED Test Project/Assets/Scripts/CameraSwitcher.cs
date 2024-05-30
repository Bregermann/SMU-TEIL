using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Unity.XR.CoreUtils;

public class CameraSwitcher : MonoBehaviour
{
    public static CameraSwitcher Instance { get; private set; }

    public List<Camera> stationaryCameras = new List<Camera>();
    public List<Camera> avatarCameras = new List<Camera>();

    private int currentStationaryIndex = 0;
    private int currentAvatarIndex = 0;

    public MiniMapController minimapController;

    private AudioListener audioListener;

    public XROrigin xrOrigin;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        // Initialize stationary cameras (assuming they are tagged as "StationaryCamera")
        stationaryCameras.AddRange(GameObject.FindGameObjectsWithTag("StationaryCamera").Select(go => go.GetComponent<Camera>()));
        stationaryCameras.Sort((cam1, cam2) => string.Compare(cam1.name, cam2.name));
        // Find or create the Audio Listener
        audioListener = FindObjectOfType<AudioListener>();
        if (audioListener == null)
        {
            audioListener = new GameObject("AudioListener").AddComponent<AudioListener>();
        }

        // Activate the first camera and move the Audio Listener
        SetActiveCamera(stationaryCameras[0]);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwitchToPreviousStationaryCamera();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwitchToNextStationaryCamera();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SwitchToNextAvatarCamera();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SwitchToPreviousAvatarCamera();
        }
    }

    private void SwitchToPreviousStationaryCamera()
    {
        if (stationaryCameras.Count == 0) return;

        currentStationaryIndex = (currentStationaryIndex - 1 + stationaryCameras.Count) % stationaryCameras.Count;
        SetActiveCamera(stationaryCameras[currentStationaryIndex]);
        minimapController.target = stationaryCameras[currentStationaryIndex].transform;
    }

    private void SwitchToNextStationaryCamera()
    {
        if (stationaryCameras.Count == 0) return;

        currentStationaryIndex = (currentStationaryIndex + 1) % stationaryCameras.Count;
        SetActiveCamera(stationaryCameras[currentStationaryIndex]);
        minimapController.target = stationaryCameras[currentStationaryIndex].transform;
    }

    private void SwitchToNextAvatarCamera()
    {
        // Initialize stationary cameras (assuming they are tagged as "AvatarCamera")
        avatarCameras.AddRange(GameObject.FindGameObjectsWithTag("AvatarCamera").Select(go => go.GetComponent<Camera>()));
        avatarCameras.Sort((cam1, cam2) => string.Compare(cam1.name, cam2.name));

        currentAvatarIndex = (currentAvatarIndex + 1) % avatarCameras.Count;
        SetActiveCamera(avatarCameras[currentAvatarIndex]);
        minimapController.target = avatarCameras[currentAvatarIndex].transform;
    }

    private void SwitchToPreviousAvatarCamera()
    {
        // Initialize stationary cameras (assuming they are tagged as "AvatarCamera")
        avatarCameras.AddRange(GameObject.FindGameObjectsWithTag("AvatarCamera").Select(go => go.GetComponent<Camera>()));
        avatarCameras.Sort((cam1, cam2) => string.Compare(cam1.name, cam2.name));

        currentAvatarIndex = (currentAvatarIndex - 1 + avatarCameras.Count) % avatarCameras.Count;
        SetActiveCamera(avatarCameras[currentAvatarIndex]);
minimapController.target = avatarCameras[currentAvatarIndex].transform;
    }

    private void SetActiveCamera(Camera cam)
    {
        foreach (Camera camera in stationaryCameras)
        {
            camera.gameObject.SetActive(camera == cam);
        }
        foreach (Camera camera in avatarCameras)
        {
            camera.gameObject.SetActive(camera == cam);
        }
        // Move the Audio Listener to the active camera
        if (audioListener != null)
        {
            audioListener.transform.SetParent(cam.transform, false);
            audioListener.transform.localPosition = Vector3.zero;
        }

        // Update the XR Origin's camera to match the active camera
        if (xrOrigin != null)
        {
            xrOrigin.Camera.transform.SetPositionAndRotation(cam.transform.position, cam.transform.rotation);
        }
    }

    public List<Camera> GetAllCameras()
    {
        List<Camera> allCameras = new List<Camera>();
        allCameras.AddRange(stationaryCameras);
        allCameras.AddRange(avatarCameras);
        return allCameras;
    }

    public Camera GetCurrentCamera()
    {
        if (stationaryCameras.Contains(Camera.main))
        {
            return stationaryCameras[currentStationaryIndex];
        }
        if (avatarCameras.Contains(Camera.main))
        {
            return avatarCameras[currentAvatarIndex];
        }
        return null;
    }

    public void RegisterAvatarCamera(Camera avatarCamera)
    {
        if (!avatarCameras.Contains(avatarCamera))
        {
            avatarCameras.Add(avatarCamera);
        }
    }
}
