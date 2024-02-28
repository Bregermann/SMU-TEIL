using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private OVRInput.Controller controller = OVRInput.Controller.None;

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

        // Check for input on X button to rewind
        if (OVRInput.GetDown(OVRInput.Button.Three, controller))
        {
            // Implement rewind functionality here
            Debug.Log("Rewind button pressed");
        }
    }
}