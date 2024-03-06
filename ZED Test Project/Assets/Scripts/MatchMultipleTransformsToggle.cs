using UnityEngine;
using UnityEngine.XR;

public class MatchMultipleTransformsToggle : MonoBehaviour
{
    public string targetTag = "TargetObject";
    private bool isMatching = false;
    private int currentIndex = 0;
    private Transform[] targetObjects;

    void Start()
    {
        // Find all game objects with the specified tag and store their transforms
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(targetTag);
        targetObjects = new Transform[objectsWithTag.Length];
        for (int i = 0; i < objectsWithTag.Length; i++)
        {
            targetObjects[i] = objectsWithTag[i].transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (targetObjects.Length == 0)
        {
            Debug.LogWarning("No target objects found with tag: " + targetTag);
            return;
        }

        if (OVRInput.Get(OVRInput.Button.One))
        {
            isMatching = !isMatching;
            if (isMatching)
            {
                currentIndex++;
                if (currentIndex >= targetObjects.Length)
                {
                    currentIndex = 0;
                }
                MatchTransform();
            }
        }
    }
    void MatchTransform()
    {
        // Match the position and rotation of the current target object
        Transform currentTarget = targetObjects[currentIndex];
        transform.position = currentTarget.position;
        transform.rotation = currentTarget.rotation;
    }
}
