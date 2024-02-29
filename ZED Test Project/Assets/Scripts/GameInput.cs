using Assets.OVR.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private OVRInput.Controller controller = OVRInput.Controller.None;
    public float moveSpeed = 3f; // Speed of player movement
  
    private void Start()
    {
        // Determine which controller is connected
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LHand))
        {
            controller = OVRInput.Controller.LHand;
        }
        else if (OVRInput.IsControllerConnected(OVRInput.Controller.RHand))
        {
            controller = OVRInput.Controller.RHand;
        }
    }

    private void Update()
    {
        // Check for input on Y button to pause and play
        if (OVRInput.GetDown(OVRInput.Button.Four, controller))
        {
            if (Time.timeScale == 0f)
            {
                Time.timeScale = 1f; // Resume game
            }
            else
            {
                Time.timeScale = 0f; // Pause game
            }
        }

        // Get thumbstick input from left controller
        Vector2 thumbstickInputLeft = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.LTouch);

        // Calculate movement direction based on thumbstick input
        Vector3 moveDirection = new Vector3(thumbstickInputLeft.x, 0f, thumbstickInputLeft.y).normalized;

        // Move the player in the calculated direction
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}