using UnityEngine;
using System.Collections.Generic;

public class MinimapManager : MonoBehaviour
{
    public RectTransform minimapPanel;
    public GameObject viewConePrefab; // Prefab for the view cone sprite

    private List<GameObject> entityIcons = new List<GameObject>();
    private GameObject viewConeObject;

    private void Start()
    {
        FindAndDisplayEntities();
        CreateViewCone();
    }

    private void Update()
    {
        UpdateViewCone();
    }

    private void FindAndDisplayEntities()
    {
        MiniMapEntity[] entities = FindObjectsOfType<MiniMapEntity>();

        foreach (MiniMapEntity entity in entities)
        {
            GameObject icon = new GameObject("EntityIcon", typeof(RectTransform));
            icon.transform.SetParent(minimapPanel);
            SpriteRenderer iconRenderer = icon.AddComponent<SpriteRenderer>();
            iconRenderer.sprite = entity.icon;
            icon.transform.localPosition = GetEntityPositionOnMinimap(entity.transform.position);
            entityIcons.Add(icon);
        }
    }

    private Vector2 GetEntityPositionOnMinimap(Vector3 entityPosition)
    {
        Vector3 localPosition = minimapPanel.InverseTransformPoint(entityPosition);
        return new Vector2(localPosition.x, localPosition.z);
    }

    private void CreateViewCone()
    {
        if (viewConePrefab != null)
        {
            viewConeObject = Instantiate(viewConePrefab, minimapPanel);
            viewConeObject.SetActive(false); // Initially hide the view cone
        }
        else
        {
            Debug.LogError("View cone prefab is not assigned.");
        }
    }

    private void UpdateViewCone()
    {
        MiniMapEntity activeEntity = FindObjectOfType<MiniMapEntity>();

        if (activeEntity != null)
        {
            viewConeObject.SetActive(true);
            viewConeObject.transform.localPosition = GetEntityPositionOnMinimap(activeEntity.transform.position);
            viewConeObject.transform.localRotation = Quaternion.Euler(0, 0, activeEntity.rotation); // Use rotation property
        }
        else
        {
            viewConeObject.SetActive(false);
        }
    }
}
