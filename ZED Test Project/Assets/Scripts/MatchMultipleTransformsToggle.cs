using UnityEngine;
using UnityEngine.XR;

public class MatchMultipleTransformsToggle : MonoBehaviour
{
    public string targetTag = "head";
    private bool isMatching = false;
    private int currentIndex = 0;
    private Transform[] targetObjects;
    public Camera camCam;
    public Transform currentTarget;

    void Start()
    {
        Camera.main.transform.SetParent(camCam.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Button.Two) || Input.GetKeyDown(KeyCode.S)) 
        {
            LookForHead();
        }
        if (OVRInput.Get(OVRInput.Button.One) || Input.GetKey(KeyCode.A))
        {
            isMatching = !isMatching;
            if (isMatching)
            {
                currentIndex++;
                if (currentIndex >= targetObjects.Length)
                {
                    currentIndex = 0;
                }
            }
        }
        if(currentTarget != null)
        {
            MatchTransform();
        }
    }
    void MatchTransform()
    {
        // Match the position and rotation of the current target object
        transform.position = currentTarget.position;
        transform.rotation = currentTarget.rotation;
    }
    void LookForHead()
    {
        // Find all game objects with the specified tag and store their transforms
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(targetTag);
        targetObjects = new Transform[objectsWithTag.Length];
        for (int i = 0; i < objectsWithTag.Length; i++)
        {
            targetObjects[i] = objectsWithTag[i].transform;
        }
        currentTarget = targetObjects[currentIndex];
    }
}
