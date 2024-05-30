using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils; // For XROrigin
using UnityEngine.XR; // For InputDevice
using UnityEngine.XR.Interaction.Toolkit; // For XR Controller inputs

public class CameraSwitcherVR : MonoBehaviour
{
    public static CameraSwitcherVR Instance { get; private set; }

    public List<Transform> stationaryCameraTransforms = new List<Transform>();
    public List<Transform> avatarCameraTransforms = new List<Transform>();

    private int currentStationaryIndex = 0;
    private int currentAvatarIndex = 0;

    public MiniMapController minimapController;
    public XROrigin xrOrigin; // Reference to the XR Origin

    private InputDevice leftController;
    private InputDevice rightController;

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
        stationaryCameraTransforms.AddRange(GameObject.FindGameObjectsWithTag("StationaryCamera").Select(go => go.transform));
        stationaryCameraTransforms.Sort((trans1, trans2) => string.Compare(trans1.name, trans2.name));

        // Initialize avatar cameras (assuming they are tagged as "AvatarCamera")
        avatarCameraTransforms.AddRange(GameObject.FindGameObjectsWithTag("AvatarCamera").Select(go => go.transform));
        avatarCameraTransforms.Sort((trans1, trans2) => string.Compare(trans1.name, trans2.name));

        // Get the left and right controllers
        leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        // Activate the first camera transform
        if (stationaryCameraTransforms.Count > 0)
        {
            SetActiveCameraTransform(stationaryCameraTransforms[0]);
        }
    }

    private void Update()
    {
        // Check if left or right controller buttons are pressed
        if (leftController.isValid)
        {
            leftController.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftPrimaryButton);
            leftController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool leftSecondaryButton);

            if (leftPrimaryButton)
            {
                SwitchToPreviousStationaryCamera();
            }
            if (leftSecondaryButton)
            {
                SwitchToNextStationaryCamera();
            }
        }

        if (rightController.isValid)
        {
            rightController.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightPrimaryButton);
            rightController.TryGetFeatureValue(CommonUsages.secondaryButton, out bool rightSecondaryButton);

            if (rightPrimaryButton)
            {
                SwitchToNextAvatarCamera();
            }
            if (rightSecondaryButton)
            {
                SwitchToPreviousAvatarCamera();
            }
        }
    }

    private void SwitchToPreviousStationaryCamera()
    {
        if (stationaryCameraTransforms.Count == 0) return;

        currentStationaryIndex = (currentStationaryIndex - 1 + stationaryCameraTransforms.Count) % stationaryCameraTransforms.Count;
        SetActiveCameraTransform(stationaryCameraTransforms[currentStationaryIndex]);
        if (minimapController != null)
        {
            minimapController.target = stationaryCameraTransforms[currentStationaryIndex];
        }
    }

    private void SwitchToNextStationaryCamera()
    {
        if (stationaryCameraTransforms.Count == 0) return;

        currentStationaryIndex = (currentStationaryIndex + 1) % stationaryCameraTransforms.Count;
        SetActiveCameraTransform(stationaryCameraTransforms[currentStationaryIndex]);
        if (minimapController != null)
        {
            minimapController.target = stationaryCameraTransforms[currentStationaryIndex];
        }
    }

    private void SwitchToNextAvatarCamera()
    {
        if (avatarCameraTransforms.Count == 0) return;

        currentAvatarIndex = (currentAvatarIndex + 1) % avatarCameraTransforms.Count;
        SetActiveCameraTransform(avatarCameraTransforms[currentAvatarIndex]);
        if (minimapController != null)
        {
            minimapController.target = avatarCameraTransforms[currentAvatarIndex];
        }
    }

    private void SwitchToPreviousAvatarCamera()
    {
        if (avatarCameraTransforms.Count == 0) return;

        currentAvatarIndex = (currentAvatarIndex - 1 + avatarCameraTransforms.Count) % avatarCameraTransforms.Count;
        SetActiveCameraTransform(avatarCameraTransforms[currentAvatarIndex]);
        if (minimapController != null)
        {
            minimapController.target = avatarCameraTransforms[currentAvatarIndex];
        }
    }

    private void SetActiveCameraTransform(Transform targetTransform)
    {
        if (xrOrigin != null)
        {
            xrOrigin.Camera.transform.SetPositionAndRotation(targetTransform.position, targetTransform.rotation);
        }
    }

    public List<Transform> GetAllCameraTransforms()
    {
        List<Transform> allCameraTransforms = new List<Transform>();
        allCameraTransforms.AddRange(stationaryCameraTransforms);
        allCameraTransforms.AddRange(avatarCameraTransforms);
        return allCameraTransforms;
    }

    public Transform GetCurrentCameraTransform()
    {
        if (stationaryCameraTransforms.Contains(xrOrigin.Camera.transform))
        {
            return stationaryCameraTransforms[currentStationaryIndex];
        }
        if (avatarCameraTransforms.Contains(xrOrigin.Camera.transform))
        {
            return avatarCameraTransforms[currentAvatarIndex];
        }
        return null;
    }

    public void RegisterAvatarCameraTransform(Transform avatarCameraTransform)
    {
        if (!avatarCameraTransforms.Contains(avatarCameraTransform))
        {
            avatarCameraTransforms.Add(avatarCameraTransform);
        }
    }
}
