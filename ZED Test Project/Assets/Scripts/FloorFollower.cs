using UnityEngine;
using System.Collections.Generic;

public class FloorFollower : MonoBehaviour
{
    public float offset = 0.1f; // Offset below the lowest foot
    private static List<Foot> footObjects = new List<Foot>();
    private static FloorFollower instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public static void RegisterFoot(Foot foot)
    {
        footObjects.Add(foot);
        instance.UpdateFloorPosition();
    }

    public static void UnregisterFoot(Foot foot)
    {
        footObjects.Remove(foot);
        instance.UpdateFloorPosition();
    }

    private void UpdateFloorPosition()
    {
        if (footObjects.Count == 0)
        {
            Debug.LogWarning("No foot objects found.");
            return;
        }

        // Initialize the lowest Y position to a large positive value
        float lowestFootY = Mathf.Infinity;

        // Iterate through each foot object and find the lowest one
        foreach (Foot foot in footObjects)
        {
            float footY = foot.transform.position.y;
            if (footY < lowestFootY)
            {
                lowestFootY = footY;
            }
        }

        // Set the floor position to just below the lowest foot
        Vector3 floorPosition = transform.position;
        floorPosition.y = lowestFootY - offset;
        transform.position = floorPosition;
    }
}
