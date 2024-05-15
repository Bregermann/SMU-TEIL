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
    private CameraSwitcher cameraSwitcher;

    private void Start()
    {
        cameraSwitcher = FindObjectOfType<CameraSwitcher>();
        if (cameraSwitcher == null)
        {
            Debug.LogError("CameraSwitcher not found in the scene.");
            return;
        }

        CreateCameraIcons();
    }

    private void Update()
    {
        UpdateCameraIcons();
    }

    private void CreateCameraIcons()
    {
        foreach (Camera cam in cameraSwitcher.GetCameras())
        {
            GameObject icon = Instantiate(cameraIconPrefab, minimapPanel);
            cameraIcons.Add(icon);
        }
    }

    private void UpdateCameraIcons()
    {
        for (int i = 0; i < cameraSwitcher.GetCameras().Count; i++)
        {
            Camera cam = cameraSwitcher.GetCameras()[i];
            RectTransform iconRect = cameraIcons[i].GetComponent<RectTransform>();
            Vector3 iconPosition = minimapPanel.InverseTransformPoint(cam.transform.position);
            iconRect.anchoredPosition = new Vector2(iconPosition.x, iconPosition.z);

            Image iconImage = cameraIcons[i].GetComponent<Image>();
            iconImage.sprite = (i == cameraSwitcher.GetCurrentCameraIndex()) ? activeCameraSprite : inactiveCameraSprite;

            DrawCameraCone(iconRect, cam.transform);
        }
    }

    private void DrawCameraCone(RectTransform iconRect, Transform cameraTransform)
    {
        Vector3 coneStart = iconRect.anchoredPosition;
        Vector3 coneEnd = coneStart + new Vector3(cameraTransform.forward.x, cameraTransform.forward.z) * coneLength;

        // Draw the cone with lines
        Debug.DrawLine(coneStart, coneEnd, coneColor);

        Vector3 leftConeEdge = Quaternion.Euler(0, -coneWidth, 0) * cameraTransform.forward * coneLength;
        Vector3 rightConeEdge = Quaternion.Euler(0, coneWidth, 0) * cameraTransform.forward * coneLength;

        Debug.DrawLine(coneStart, coneStart + leftConeEdge, coneColor);
        Debug.DrawLine(coneStart, coneStart + rightConeEdge, coneColor);
    }
}
