using UnityEngine;

public class MatchTransform : MonoBehaviour
{
    public Transform targetObject;

    // Update is called once per frame
    void Update()
    {
        if (targetObject != null)
        {
            // Match the position and rotation of the target object
            transform.position = targetObject.position;
            transform.rotation = targetObject.rotation;
        }
        else
        {
            Debug.LogWarning("Target object is not assigned!");
        }
    }
}