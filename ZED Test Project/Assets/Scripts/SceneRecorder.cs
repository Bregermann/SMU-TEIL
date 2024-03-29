using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SceneRecorder : MonoBehaviour
{
    public string saveFileName = "sceneData.json";
    public KeyCode startRecordingKey = KeyCode.R;
    public KeyCode stopRecordingKey = KeyCode.S;

    private List<RecordedObjectData> recordedData = new List<RecordedObjectData>();
    private bool isRecording = false;

    [System.Serializable]
    public class RecordedObjectData
    {
        public string name;
        public string tag;
        public List<KeyframeData> keyframes = new List<KeyframeData>();
    }

    [System.Serializable]
    public class KeyframeData
    {
        public float time;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
    }

    void Update()
    {
        if (Input.GetKeyDown(startRecordingKey))
        {
            StartRecording();
        }
        else if (Input.GetKeyDown(stopRecordingKey))
        {
            StopRecording();
        }

        if (isRecording)
        {
            RecordFrame();
        }
    }

    void StartRecording()
    {
        recordedData.Clear();
        isRecording = true;
    }

    void StopRecording()
    {
        isRecording = false;
        SaveRecordedData();
    }

    void RecordFrame()
    {
        RecordedObjectData frameData = new RecordedObjectData();

        foreach (GameObject obj in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
        {
            RecordObject(obj, frameData);
        }

        recordedData.Add(frameData);
    }

    void RecordObject(GameObject obj, RecordedObjectData frameData)
    {
        KeyframeData keyframe = new KeyframeData();
        keyframe.time = Time.time;
        keyframe.position = obj.transform.position;
        keyframe.rotation = obj.transform.rotation;
        keyframe.scale = obj.transform.localScale;

        frameData.name = obj.name;
        frameData.tag = obj.tag;
        frameData.keyframes.Add(keyframe);

        foreach (Transform child in obj.transform)
        {
            RecordObject(child.gameObject, frameData);
        }
    }

    void SaveRecordedData()
    {
        string subDirectory = "RecordedData"; // Specify the subdirectory name
        string filePath = Path.Combine(Application.dataPath, subDirectory, saveFileName);
        string directoryPath = Path.Combine(Application.dataPath, subDirectory);

        // Create the directory if it doesn't exist
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string json = JsonUtility.ToJson(recordedData.ToArray());
        File.WriteAllText(filePath, json);

        Debug.Log("Recorded data saved to: " + filePath);
    }

}
