using UnityEngine;

public class Foot : MonoBehaviour
{
    private void OnEnable()
    {
        FloorFollower.RegisterFoot(this);
    }

    private void OnDisable()
    {
        FloorFollower.UnregisterFoot(this);
    }
}
