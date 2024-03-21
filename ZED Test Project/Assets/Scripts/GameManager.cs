using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public ZEDManager zedManager;
    public ZEDBodyTrackingManager bodyTrackingManager;
    public GameObject uiButtons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Z))
        {
            uiButtons.SetActive(false);
        }
        if(Input.GetKey(KeyCode.X))
        {
            uiButtons.SetActive(true);
        }
    }
    public void StartBodyTracking()
    {
        zedManager.StartBodyTracking();
    }
    public void StopBodyTracking()
    {
        zedManager.StopBodyTracking();
    }
    public void StartRecord()
    {
        //start recording stereolabs cameras
    }
    public void StopRecord()
    {
        //stop recording stereolabs cameras
        //pop up two buttons, one to save, one to not
    }
   public void SaveRecording()
    {
        //save the recording to a specified place on the hard drive or server
    }
    public void StartSpatialMapping()
    {
        zedManager.StartSpatialMapping();
    }
    public void StopSpatialMapping()
    {
        zedManager.StopSpatialMapping();
    }
    public void SaveMesh()
    {
        zedManager.SaveMesh();
    }
    public void StartObjectDetection()
    {
        zedManager.StartObjectDetection(); ;
    }
    public void StopObjectDetection()
    {
        zedManager.StopObjectDetection();
    }
    public void ToggleUI()
    {
        //toggle display of UI on and off
    }
    public void ExportAnimationFromSVO()
    {
        //find a way to take the animation created from the svo, either during recording or during playback
    }

}
