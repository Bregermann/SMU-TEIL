using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public ZEDManager zedManager;
    public ZEDBodyTrackingManager bodyTrackingManager;
    public GameObject uiButtons;
    public GameObject threeDObjectVisualizer;
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void BackToCharacterSelect()
    {
        SceneManager.LoadScene(0);
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
        //stop recording stereolabs cameras and auto save
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
        zedManager.StartObjectDetection(); 
        threeDObjectVisualizer.SetActive(true);
    }
    public void StopObjectDetection()
    {
        zedManager.StopObjectDetection();
        threeDObjectVisualizer.SetActive(false);
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
