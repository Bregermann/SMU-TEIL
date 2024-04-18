using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f; // Adjust this to change the speed of rotation

    void Update()
    {
        // Rotate the GameObject around its Y axis continuously
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}