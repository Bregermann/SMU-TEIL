using Assets.OVR.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBody : MonoBehaviour
{
    private OVRInput.Controller controller = OVRInput.Controller.None;
    private bool isRewinding;
    List<PointInTime> pointsInTime;

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

        pointsInTime = new List<PointInTime>();
    }

    private void Update()
    {


        // Check for input on X button to rewind
        if (OVRInput.GetDown(OVRInput.Button.Three, controller))
        {
            // Implement rewind functionality here
            Debug.Log("Rewind button pressed");
            if (isRewinding == true)
            {
                StopRewind();
            }
            else
            {
                StartRewind();
            }

        }
    }
    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }
    void Record()
    {
        pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation));
    }
    void Rewind()
    {
        if(pointsInTime.Count > 0)
        {
            PointInTime pointInTime = pointsInTime[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            pointsInTime.RemoveAt(0);
        }
        else
        {
            StopRewind();
        }
    }
    void StartRewind()
    {
        isRewinding = true;
    }
    void StopRewind()
    {
        isRewinding = false;
    }
}