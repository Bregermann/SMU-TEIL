using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class CameraSwitcher : MonoBehaviour
{
    public static CameraSwitcher Instance { get; private set; }

    public List<Camera> stationaryCameras = new List<Camera>();
    public List<Camera> avatarCameras = new List<Camera>();

    private int currentStationaryIndex = 0;
    private int currentAvatarIndex = 0;

    public MiniMapController minimapController;

    private AudioListener audioListener;

    public GameObject camera1;
    public GameObject camera2;
    public GameObject camera3;
    public GameObject camera4;
    public GameObject camera9;
    private bool previousActiveState1;
    private bool previousActiveState2;
    private bool previousActiveState3;
    private bool previousActiveState4;
    private bool previousActiveState9;

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
    private void Start()
    {
        previousActiveState1 = camera1.activeSelf;
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
        if (camera1.activeSelf != previousActiveState1)
        {
            previousActiveState1 = camera1.activeSelf;

            if (camera1.activeSelf)
            {
                ChangeViewConeOffset1();
            }
        }
        if (camera2.activeSelf != previousActiveState2)
        {
            previousActiveState2 = camera2.activeSelf;

            if (camera2.activeSelf)
            {
                ChangeViewConeOffset2();
            }
        }
        if (camera3.activeSelf != previousActiveState3)
        {
            previousActiveState3 = camera3.activeSelf;

            if (camera3.activeSelf)
            {
                ChangeViewConeOffset3();
            }
        }
        if (camera4.activeSelf != previousActiveState4)
        {
            previousActiveState4 = camera4.activeSelf;

            if (camera4.activeSelf)
            {
                ChangeViewConeOffset4();
            }
        }
        if (camera9.activeSelf != previousActiveState9)
        {
            previousActiveState9 = camera9.activeSelf;

            if (camera9.activeSelf)
            {
                ChangeViewConeOffset4();
            }
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
        // Clear the list to avoid adding duplicate cameras
        avatarCameras.Clear();

        // Initialize avatar cameras (assuming they are tagged as "AvatarCamera")
        avatarCameras.AddRange(GameObjectFinder.FindGameObjectsWithTag("AvatarCamera").Select(go => go.GetComponent<Camera>()));
        avatarCameras.Sort((cam1, cam2) => string.Compare(cam1.name, cam2.name));

        if (avatarCameras.Count == 0) return;

        currentAvatarIndex = (currentAvatarIndex + 1) % avatarCameras.Count;
        SetActiveCamera(avatarCameras[currentAvatarIndex]);
        minimapController.target = avatarCameras[currentAvatarIndex].transform;
    }

    private void SwitchToPreviousAvatarCamera()
    {
        // Clear the list to avoid adding duplicate cameras
        avatarCameras.Clear();

        // Initialize avatar cameras (assuming they are tagged as "AvatarCamera")
        avatarCameras.AddRange(GameObjectFinder.FindGameObjectsWithTag("AvatarCamera").Select(go => go.GetComponent<Camera>()));
        avatarCameras.Sort((cam1, cam2) => string.Compare(cam1.name, cam2.name));

        if (avatarCameras.Count == 0) return;

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
    public static class GameObjectFinder
    {
        public static List<GameObject> FindGameObjectsWithTag(string tag)
        {
            List<GameObject> taggedObjects = new List<GameObject>();
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                if (obj.CompareTag(tag))
                {
                    // Ensure the object is part of the active scene (not an asset in the editor)
                    if (obj.scene.isLoaded)
                    {
                        taggedObjects.Add(obj);
                    }
                }
            }

            return taggedObjects;
        }
    }
    void ChangeViewConeOffset1()
    {
        if (minimapController != null)
        {
            // Change the X and Z values of viewConeOffset
            minimapController.viewConeOffset = new Vector3(1f, minimapController.viewConeOffset.y, 3.5f); // Example values
            Debug.Log("viewConeOffset has been changed to: " + minimapController.viewConeOffset);
        }
        else
        {
            Debug.LogWarning("MiniMapController reference is not set.");
        }
    }
    void ChangeViewConeOffset2()
    {
        if (minimapController != null)
        {
            // Change the X and Z values of viewConeOffset
            minimapController.viewConeOffset = new Vector3(.25f, minimapController.viewConeOffset.y, 2f); // Example values
            Debug.Log("viewConeOffset has been changed to: " + minimapController.viewConeOffset);
        }
        else
        {
            Debug.LogWarning("MiniMapController reference is not set.");
        }
    }
    void ChangeViewConeOffset3()
    {
        if (minimapController != null)
        {
            // Change the X and Z values of viewConeOffset
            minimapController.viewConeOffset = new Vector3(-2f, minimapController.viewConeOffset.y, 3.5f); // Example values
            Debug.Log("viewConeOffset has been changed to: " + minimapController.viewConeOffset);
        }
        else
        {
            Debug.LogWarning("MiniMapController reference is not set.");
        }
    }
    void ChangeViewConeOffset4()
    {
        if (minimapController != null)
        {
            // Change the X and Z values of viewConeOffset
            minimapController.viewConeOffset = new Vector3(0f, minimapController.viewConeOffset.y, 0f); // Example values
            Debug.Log("viewConeOffset has been changed to: " + minimapController.viewConeOffset);
        }
        else
        {
            Debug.LogWarning("MiniMapController reference is not set.");
        }
    }
}
