using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[ExecuteInEditMode]
public class MiniMapController : MonoBehaviour
{
    [HideInInspector] public Transform shapeColliderGO;
    [HideInInspector] public RenderTexture renderTex;
    [HideInInspector] public Material mapMaterial;
    [HideInInspector] public List<MiniMapEntity> miniMapEntities;
    [HideInInspector] public GameObject iconPref;
    [HideInInspector] public Camera mapCamera;

    [Tooltip("The target which the minimap will be following")]
    public Transform target;
    [Tooltip("Set which layers to show in the minimap")]
    public LayerMask minimapLayers;
    [Tooltip("Set this true, if you want minimap border as background of minimap")]
    public bool showBackground;
    [Tooltip("The mask to change the shape of minimap")]
    public Sprite miniMapMask;
    [Tooltip("Border graphics of the minimap")]
    public Sprite miniMapBorder;
    [Tooltip("Set opacity of minimap")]
    [Range(0, 1)]
    public float miniMapOpacity = 1;
    [Tooltip("Border graphics of the minimap")]
    public Vector3 miniMapScale = new Vector3(1, 1, 1);
    [Tooltip("Camera offset from the target")]
    public Vector3 cameraOffset = new Vector3(0f, 7.5f, 0f);
    [Tooltip("Camera's orthographic size")]
    public float camSize = 15;
    [Tooltip("Camera's far clip")]
    public float camFarClip = 1000;
    [Tooltip("Adjust the rotation according to your scene")]
    public Vector3 rotationOfCam = new Vector3(90, 0, 0);
    [Tooltip("If true the camera rotates according to the target")]
    public bool rotateWithTarget = true;
    [Tooltip("The position of the minimap camera")]
    public Vector3 minimapCameraPosition = new Vector3(0f, 7.5f, 0f);
    [Tooltip("Prefab for the view cone")]
    public GameObject viewConePrefab;
    private GameObject viewConeInstance;
    [Tooltip("Layer for the view cone")]
    public LayerMask viewConeLayer;
    [Tooltip("Offset for the view cone relative to the target")]
    public Vector3 viewConeOffset = new Vector3(0f, 7.5f, 0f);

    private Dictionary<GameObject, GameObject> ownerIconMap = new Dictionary<GameObject, GameObject>();

    private GameObject miniMapPanel;
    private Image mapPanelMask;
    private Image mapPanelBorder;
    private Image mapPanel;
    private Color mapColor;
    private Color mapBorderColor;

    private RectTransform mapPanelRect;
    private RectTransform mapPanelMaskRect;

    private Vector3 prevRotOfCam;
    private Vector2 res;
    private Image miniMapPanelImage;

    [SerializeField] private string tagName = "ViewCone";

    // Initialize everything here
    public void OnEnable()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tagName);
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
        ownerIconMap.Clear();
        GameObject maskPanelGO = transform.GetComponentInChildren<Mask>().gameObject;
        mapPanelMask = maskPanelGO.GetComponent<Image>();
        mapPanelBorder = maskPanelGO.transform.parent.GetComponent<Image>();
        miniMapPanel = maskPanelGO.transform.GetChild(0).gameObject;
        mapPanel = miniMapPanel.GetComponent<Image>();
        mapColor = mapPanel.color;
        mapBorderColor = mapPanelBorder.color;

        if (mapCamera == null) mapCamera = transform.GetComponentInChildren<Camera>();
        mapCamera.cullingMask = minimapLayers;

        mapPanelMaskRect = maskPanelGO.GetComponent<RectTransform>();
        mapPanelRect = miniMapPanel.GetComponent<RectTransform>();
        mapPanelRect.anchoredPosition = mapPanelMaskRect.anchoredPosition;
        res = new Vector2(Screen.width, Screen.height);

        miniMapPanelImage = miniMapPanel.GetComponent<Image>();
        miniMapPanelImage.enabled = !showBackground;
        SetupRenderTexture();

        // Initialize the view cone
        if (viewConePrefab != null && viewConeInstance == null)
        {
            viewConeInstance = Instantiate(viewConePrefab, miniMapPanel.transform);
            viewConeInstance.transform.localPosition = Vector3.zero; // Ensure it is centered on the minimap
            SetLayerRecursively(viewConeInstance, LayerMask.NameToLayer("ViewConeLayer"));
        }

        // Register all MiniMapComponent objects in the scene
        RegisterAllMiniMapComponents();
    }

    // Release the unmanaged objects
    void OnDisable()
    {
        if (renderTex != null)
        {
            if (!renderTex.IsCreated())
            {
                renderTex.Release();
            }
        }

        // Destroy the view cone instance if it exists
        if (viewConeInstance != null)
        {
            DestroyImmediate(viewConeInstance);
        }

        // Unregister all MiniMapComponent objects
        UnregisterAllMiniMapComponents();
    }

    // Release the unmanaged objects
    void OnDestroy()
    {
        if (renderTex != null)
        {
            if (!renderTex.IsCreated())
            {
                renderTex.Release();
            }
        }

        // Destroy the view cone instance if it exists
        if (viewConeInstance != null)
        {
            DestroyImmediate(viewConeInstance);
        }
    }

    // As this script is ExecuteInEditMode, this function will be called when something in scene changes
    public void LateUpdate()
    {
        mapPanelMask.sprite = miniMapMask;
        mapPanelBorder.sprite = miniMapBorder;
        mapPanelBorder.rectTransform.localScale = miniMapScale;
        mapBorderColor.a = miniMapOpacity;
        mapColor.a = miniMapOpacity;
        mapPanelBorder.color = mapBorderColor;
        mapPanel.color = mapColor;

        mapPanelMaskRect.sizeDelta = new Vector2(Mathf.RoundToInt(mapPanelMaskRect.sizeDelta.x), Mathf.RoundToInt(mapPanelMaskRect.sizeDelta.y));
        mapPanelRect.position = mapPanelMaskRect.position;
        mapPanelRect.sizeDelta = mapPanelMaskRect.sizeDelta;
        miniMapPanelImage.enabled = !showBackground;

        if (Screen.width != res.x || Screen.height != res.y)
        {
            SetupRenderTexture();
            res.x = Screen.width;
            res.y = Screen.height;
        }
        SetCam();

        // Update the view cone position and rotation
        if (viewConeInstance != null && target != null)
        {
            UpdateViewCone();
        }
    }

    void SetupRenderTexture()
    {
        if (renderTex.IsCreated()) renderTex.Release();
        renderTex = new RenderTexture((int)mapPanelRect.sizeDelta.x, (int)mapPanelRect.sizeDelta.y, 24);
        renderTex.Create();

        mapMaterial.mainTexture = renderTex;
        mapCamera.targetTexture = renderTex;

        mapPanelMaskRect.gameObject.SetActive(false);
        mapPanelMaskRect.gameObject.SetActive(true);
    }

    void SetCam()
    {
        mapCamera.orthographicSize = camSize;
        mapCamera.farClipPlane = camFarClip;
        mapCamera.transform.position = minimapCameraPosition;
        mapCamera.transform.eulerAngles = rotationOfCam;
    }

    void UpdateViewCone()
    {
        if (viewConeInstance != null && target != null)
        {
            Vector3 viewConePosition = target.position + viewConeOffset;
            viewConeInstance.transform.position = viewConePosition;

            // Calculate the rotation angle based on the target's forward direction
            float viewConeYRotation = Quaternion.LookRotation(target.forward).eulerAngles.y;
            viewConeInstance.transform.rotation = Quaternion.Euler(90, viewConeYRotation, 0);
        }
    }


    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    // Register all MiniMapComponent objects in the scene
    void RegisterAllMiniMapComponents()
    {
        MiniMapComponent[] components = FindObjectsOfType<MiniMapComponent>();
        foreach (var component in components)
        {
            RegisterMapObject(component.gameObject, component.mme);
        }
    }

    // Unregister all MiniMapComponent objects
    void UnregisterAllMiniMapComponents()
    {
        foreach (var kvp in ownerIconMap)
        {
            Destroy(kvp.Value);
        }
        ownerIconMap.Clear();
    }

    // Register a minimap object here
    public MapObject RegisterMapObject(GameObject owner, MiniMapEntity mme)
    {
        GameObject curMGO = Instantiate(iconPref);
        MapObject curMO = curMGO.AddComponent<MapObject>();

        // Set the icon from MiniMapEntity
        Image iconImage = curMGO.GetComponent<Image>();
        if (iconImage != null && mme.icon != null)
        {
            iconImage.sprite = mme.icon;
        }

        curMO.SetMiniMapEntityValues(this, mme, owner, mapCamera, miniMapPanel);
        ownerIconMap.Add(owner, curMGO);
        return owner.GetComponent<MapObject>();
    }

    // Unregister a minimap object here
    public void UnregisterMapObject(MapObject mmo, GameObject owner)
    {
        if (ownerIconMap.ContainsKey(owner))
        {
            Destroy(ownerIconMap[owner]);
            ownerIconMap.Remove(owner);
        }
        Destroy(mmo);
    }
}

