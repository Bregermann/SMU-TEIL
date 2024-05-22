using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FreeFlyingController : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float rotationSpeed = 100f;
    public Camera attachedCamera;

    private float yaw = 0f;
    private float pitch = 0f;

    private InputDevice rightController;
    private InputDevice leftController;
    private InputDevice headDevice;

    void Start()
    {
        if (attachedCamera == null)
        {
            attachedCamera = Camera.main;
        }

        // Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the controllers
        var inputDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, inputDevices);
        if (inputDevices.Count > 0)
        {
            rightController = inputDevices[0];
        }

        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, inputDevices);
        if (inputDevices.Count > 0)
        {
            leftController = inputDevices[0];
        }

        InputDevices.GetDevicesAtXRNode(XRNode.CenterEye, inputDevices);
        if (inputDevices.Count > 0)
        {
            headDevice = inputDevices[0];
        }
    }

    void Update()
    {
        // Handle movement with keyboard
        float moveForward = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        float moveRight = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float moveUp = 0f;

        if (Input.GetKey(KeyCode.Q))
        {
            moveUp = movementSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            moveUp = -movementSpeed * Time.deltaTime;
        }

        // Handle movement with Quest 2 analog sticks
        Vector2 leftStickValue = Vector2.zero;
        if (leftController != null)
        {
            leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftStickValue);
        }

        moveForward += leftStickValue.y * movementSpeed * Time.deltaTime;
        moveRight += leftStickValue.x * movementSpeed * Time.deltaTime;

        transform.Translate(moveRight, moveUp, moveForward);

        // Handle rotation with mouse
        yaw += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        // Handle rotation with Quest 2 right analog stick
        Vector2 rightStickValue = Vector2.zero;
        if (rightController != null)
        {
            rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out rightStickValue);
        }

        yaw += rightStickValue.x * rotationSpeed * Time.deltaTime;
        pitch -= rightStickValue.y * rotationSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        // Apply head rotation
        if (headDevice != null)
        {
            Quaternion headRotation;
            if (headDevice.TryGetFeatureValue(CommonUsages.centerEyeRotation, out headRotation))
            {
                transform.rotation = headRotation;
            }
        }
        else
        {
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }
}
