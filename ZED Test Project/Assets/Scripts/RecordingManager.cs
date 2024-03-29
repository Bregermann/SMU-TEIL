using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RecordingManager : MonoBehaviour
{
    public float recordInterval = 0.1f; // Interval between data captures
    private List<RecordedFrame> recordedFrames = new List<RecordedFrame>();
    private bool isRecording = false;

    // Update is called once per frame
    void Update()
    {
        // Start recording when 'R' key is pressed
        if (Input.GetKeyDown(KeyCode.R) && !isRecording)
        {
            StartRecording();
        }

        // Stop recording when 'S' key is pressed
        if (Input.GetKeyDown(KeyCode.S) && isRecording)
        {
            StopRecording();
        }
    }

    // Start recording
    public void StartRecording()
    {
        isRecording = true;
        recordedFrames.Clear(); // Clear previously recorded frames
        StartCoroutine(RecordData());
    }

    // Stop recording
    public void StopRecording()
    {
        isRecording = false;
        SaveRecordedData("RecordedData.json"); // Save recorded data to a file
    }

    // Coroutine to record data at regular intervals
    private IEnumerator RecordData()
    {
        while (isRecording)
        {
            RecordFrame();
            yield return new WaitForSeconds(recordInterval);
        }
    }

    // Record data for current frame
    private void RecordFrame()
    {
        RecordedFrame frame = new RecordedFrame();
        // Capture positions and rotations of all objects in the scene
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.activeSelf) // Only record active objects
            {
                RecordObjectData(obj, frame);
            }
        }

        recordedFrames.Add(frame);
    }

    // Record position and rotation data for a single object and its children
    private void RecordObjectData(GameObject obj, RecordedFrame frame)
    {
        RecordedObjectData objectData = new RecordedObjectData();
        objectData.objectName = obj.name;
        objectData.position = obj.transform.position;
        objectData.rotation = obj.transform.rotation;

        // Record position and rotation data for object's children
        Transform[] children = obj.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.gameObject.activeSelf) // Only record active children
            {
                RecordedChildData childData = new RecordedChildData();
                childData.childName = child.name;
                childData.localPosition = child.localPosition;
                childData.localRotation = child.localRotation;
                objectData.childrenData.Add(childData);
            }
        }

        frame.objectsData.Add(objectData);
    }

    // Save recorded data to a file in the persistent data path
    private void SaveRecordedData(string filename)
    {
        string filePath = Path.Combine(Application.persistentDataPath, filename);
        // Serialize recordedFrames to JSON
        string json = JsonUtility.ToJson(recordedFrames);
        // Save JSON string to a file in the persistent data path
        File.WriteAllText(filePath, json);
        Debug.Log("Recorded data saved to: " + filePath);
    }
}

[Serializable]
public class RecordedFrame
{
    public List<RecordedObjectData> objectsData = new List<RecordedObjectData>();
}

[Serializable]
public class RecordedObjectData
{
    public string objectName;
    public Vector3 position;
    public Quaternion rotation;
    public List<RecordedChildData> childrenData = new List<RecordedChildData>();
}

[Serializable]
public class RecordedChildData
{
    public string childName;
    public Vector3 localPosition;
    public Quaternion localRotation;
}
