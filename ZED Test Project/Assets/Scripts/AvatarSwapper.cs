using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSwapper : MonoBehaviour
{
    public ZEDBodyTrackingManager bodyTrackingManager;
    public GameObject[] avatars;
    private int currentIndex = 0;
    public Material[] materials;
    private int materialIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
     if(Input.GetKeyDown(KeyCode.L))
        {
            CycleToNextAvatar();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            CycleToNextMaterial();
        }
    }
    void CycleToNextAvatar()
    {
        bodyTrackingManager.avatar.SetActive(false);
        currentIndex++;
        if(currentIndex >= avatars.Length)
        {
            currentIndex = 0;
        }
        bodyTrackingManager.avatar = avatars[currentIndex];
        bodyTrackingManager.avatar.SetActive(true);

    }
    void CycleToNextMaterial()
    {
        materialIndex++;
        if(materialIndex >= materials.Length)
        {
            materialIndex = 0;
        }
        bodyTrackingManager.skeletonBaseMaterial = materials[materialIndex];
    }
}
