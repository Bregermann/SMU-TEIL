using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MinimapManager : MonoBehaviour
{
    public RectTransform minimapPanel;
    public GameObject cameraIconPrefab;
    public Sprite activeCameraSprite;
    public Sprite inactiveCameraSprite;
    public Color coneColor = Color.green;
    public float coneLength = 50f;
    public float coneWidth = 25f;

    private List<GameObject> cameraIcons = new List<GameObject>();
    private List<GameObject> cameraCones = new List<GameObject>(); // New list for camera cones
    public CameraSwitcher cameraSwitcher;

    private void Start()
    {
        CreateCameraIcons();
        CreateCameraCones(); // Create cones for each camera
    }

    private void Update()
    {
        if (cameraSwitcher == null)
        {
            Debug.LogError("CameraSwitcher not found in the scene.");
            return;
        }

        UpdateCameraIcons();
        UpdateCameraCones(); // Update cone positions and rotations
    }

    private void CreateCameraIcons()
    {
        foreach (Camera cam in cameraSwitcher.GetCameras())
        {
            GameObject icon = Instantiate(cameraIconPrefab, minimapPanel);
            cameraIcons.Add(icon);
        }
    }

    private void CreateCameraCones()
    {
        foreach (Camera cam in cameraSwitcher.GetCameras())
        {
            GameObject cone = new GameObject("CameraCone", typeof(RectTransform), typeof(Image));
            cone.transform.SetParent(minimapPanel);
            cone.GetComponent<Image>().color = coneColor;
            cone.SetActive(false); // Initially hide all cones
            cameraCones.Add(cone);
        }
    }

    private void UpdateCameraIcons()
    {
        for (int i = 0; i < cameraIcons.Count; i++) // Use cameraIcons.Count for iteration
        {
            if (i < cameraSwitcher.GetCameras().Count) // Ensure index is valid
            {
                Camera cam = cameraSwitcher.GetCameras()[i];
                RectTransform iconRect = cameraIcons[i].GetComponent<RectTransform>();
                Vector3 iconPosition = minimapPanel.InverseTransformPoint(cam.transform.position);
                iconRect.anchoredPosition = new Vector2(iconPosition.x, iconPosition.z);

                Image iconImage = cameraIcons[i].GetComponent<Image>();
                iconImage.sprite = (i == cameraSwitcher.GetCurrentCameraIndex()) ? activeCameraSprite : inactiveCameraSprite;
            }
        }
    }

    private void UpdateCameraCones()
    {
        Camera activeCamera = cameraSwitcher.GetCameras()[cameraSwitcher.GetCurrentCameraIndex()];
        if (activeCamera != null)
        {
            // If there's an active camera, show its cone and hide others
            for (int i = 0; i < cameraCones.Count; i++) // Use cameraCones.Count for iteration
            {
                if (i < cameraSwitcher.GetCameras().Count) // Ensure index is valid
                {
                    cameraCones[i].SetActive(cameraSwitcher.GetCameras()[i] == activeCamera);
                    if (cameraSwitcher.GetCameras()[i] == activeCamera)
                    {
                        UpdateActiveCameraConePosition(activeCamera, i);
                    }
                }
            }
        }
    }

    private void UpdateActiveCameraConePosition(Camera activeCamera, int index)
    {
        RectTransform coneRect = cameraCones[index].GetComponent<RectTransform>();
        Vector3 conePosition = minimapPanel.InverseTransformPoint(activeCamera.transform.position);
        coneRect.anchoredPosition = new Vector2(conePosition.x, conePosition.z);

        float angle = Mathf.Atan2(activeCamera.transform.forward.z, activeCamera.transform.forward.x) * Mathf.Rad2Deg;
        coneRect.localEulerAngles = new Vector3(0, 0, angle);

        // Adjust cone size if needed
        coneRect.sizeDelta = new Vector2(coneLength, coneWidth);
    }
}
