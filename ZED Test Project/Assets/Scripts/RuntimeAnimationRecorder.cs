using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine.Rendering;
using System;

public class RuntimeAnimationRecorder : MonoBehaviour
{
    private List<GameObject> objectsToRecord = new List<GameObject>();
    private bool isRecording;
    private Dictionary<GameObject, List<Vector3>> positionKeyframes = new Dictionary<GameObject, List<Vector3>>();
    private Dictionary<GameObject, List<Quaternion>> rotationKeyframes = new Dictionary<GameObject, List<Quaternion>>();
    private Dictionary<GameObject, List<Vector3>> scaleKeyframes = new Dictionary<GameObject, List<Vector3>>();

    // Define a dictionary to store the index for each object
    private Dictionary<GameObject, int> saveIndex = new Dictionary<GameObject, int>();



    private void Start()
    {
        isRecording = false;
        // Initialize keyframe dictionaries for identified objects
        foreach (GameObject obj in objectsToRecord)
        {
            // Initialize dictionaries for the current object
            positionKeyframes[obj] = new List<Vector3>();
            rotationKeyframes[obj] = new List<Quaternion>();
            scaleKeyframes[obj] = new List<Vector3>();

            // Debug message to check if dictionaries are initialized
            Debug.Log("Dictionaries initialized for object: " + obj.name);
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Start recording when 'Start Recording' button is pressed
        if (Input.GetKeyDown(KeyCode.R) && isRecording == false)
        {
            StartRecording();
        }

        // Stop/save recording when 'Stop Recording' button is pressed
        if (Input.GetKeyDown(KeyCode.S) && isRecording == true)
        {
            StopRecording();
        }

        // Identify objects when 'I' key is pressed
        if (Input.GetKeyDown(KeyCode.I))
        {
            IdentifyObjects();
        }

        // Record animations when recording is active
        if (isRecording == true)
        {
            RecordAnimations();
        }
    }

    // Identify gameobjects with tag "RecordMe" and write debug message
    private void IdentifyObjects()
    {
        objectsToRecord.Clear();
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag("RecordMe");
        objectsToRecord.AddRange(taggedObjects);

        // Print the names of identified objects
        foreach (GameObject obj in objectsToRecord)
        {
            Debug.Log("Identified Object: " + obj.name);
        }

        Debug.Log("Number of objects to record: " + objectsToRecord.Count);
    }


    // Start recording animations for identified objects
    private void StartRecording()
    {
        isRecording = true;
        foreach (GameObject obj in objectsToRecord)
        {
            positionKeyframes[obj] = new List<Vector3>();
            rotationKeyframes[obj] = new List<Quaternion>();
            scaleKeyframes[obj] = new List<Vector3>();
        }
    }

    // Stop recording and save animation clips
    private void StopRecording()
    {
        isRecording = false;

        foreach (GameObject obj in objectsToRecord)
        {
            AnimationClip clip = new AnimationClip();
            clip.legacy = true;
            float recordingInterval = 1f / 30f; // Adjust recording interval as needed

            // Add position keyframes
            for (int i = 0; i < positionKeyframes[obj].Count; i++)
            {
                AddKeyframe(clip, obj.transform, "localPosition", positionKeyframes[obj][i], rotationKeyframes[obj][i], i * recordingInterval);
            }

            // Add rotation keyframes
            for (int i = 0; i < rotationKeyframes[obj].Count; i++)
            {
                AddKeyframe(clip, obj.transform, "localRotation", positionKeyframes[obj][i], rotationKeyframes[obj][i], i * recordingInterval);
            }

            // Add scale keyframes
            for (int i = 0; i < scaleKeyframes[obj].Count; i++)
            {
                AddKeyframe(clip, obj.transform, "localScale", positionKeyframes[obj][i], rotationKeyframes[obj][i], i * recordingInterval);
            }

            // Increment the save index for the object
            if (!saveIndex.ContainsKey(obj))
            {
                saveIndex[obj] = 1; // Start with index 1
            }
            else
            {
                saveIndex[obj]++;
            }

            // Generate the filename with an incremental index
            string filename = obj.name + "_" + saveIndex[obj] + ".anim";


            // Save AnimationClip to a file
            string filePath = "Assets/RecordedAnimations/" + obj.name + ".anim";
     //       AssetDatabase.CreateAsset(clip, filePath);
            Debug.Log("Saved animation clip to: " + filePath);
        }

        // Clear keyframe lists
        positionKeyframes.Clear();
        rotationKeyframes.Clear();
        scaleKeyframes.Clear();
    }

    // Record animations for identified objects and their children recursively
    private void RecordAnimations()
    {
        foreach (GameObject obj in objectsToRecord)
        {
            // Recursively record animations for all children
            RecordAnimationsRecursive(obj.transform);
        }
    }

    // Recursive function to record animations for all children of a given transform
    private void RecordAnimationsRecursive(Transform parentTransform)
    {
        // Get local position, rotation, and scale of the parent
        Vector3 localPosition = parentTransform.localPosition;
        Quaternion localRotation = parentTransform.localRotation;
        Vector3 localScale = parentTransform.localScale;

        // Add parent object to dictionaries if not already present
        if (!positionKeyframes.ContainsKey(parentTransform.gameObject))
        {
            positionKeyframes[parentTransform.gameObject] = new List<Vector3>();
            rotationKeyframes[parentTransform.gameObject] = new List<Quaternion>();
            scaleKeyframes[parentTransform.gameObject] = new List<Vector3>();
        }

        // Add parent object's transform to keyframe lists
        positionKeyframes[parentTransform.gameObject].Add(localPosition);
        rotationKeyframes[parentTransform.gameObject].Add(localRotation);
        scaleKeyframes[parentTransform.gameObject].Add(localScale);

        // Debug message to check if parent object is added to dictionaries
        Debug.Log("Parent object added to dictionaries: " + parentTransform.gameObject.name);

        // Recursively record animations for all children
        foreach (Transform childTransform in parentTransform)
        {
            RecordAnimationsRecursive(childTransform);
        }
    }

    // Add keyframe to the AnimationClip for position, rotation, or scale
    private void AddKeyframe(AnimationClip clip, Transform transform, string property, Vector3 value, Quaternion rotation, float time)
    {
        // Add keyframe for position or scale
        if (property == "localPosition" || property == "localScale")
        {
            AnimationCurve curveX = new AnimationCurve();
            curveX.AddKey(new Keyframe(time, value.x));
            clip.SetCurve("", typeof(Transform), property + ".x", curveX);

            AnimationCurve curveY = new AnimationCurve();
            curveY.AddKey(new Keyframe(time, value.y));
            clip.SetCurve("", typeof(Transform), property + ".y", curveY);

            AnimationCurve curveZ = new AnimationCurve();
            curveZ.AddKey(new Keyframe(time, value.z));
            clip.SetCurve("", typeof(Transform), property + ".z", curveZ);
        }
        // Add keyframe for rotation
        else if (property == "localRotation")
        {
            AnimationCurve curveX = new AnimationCurve();
            curveX.AddKey(new Keyframe(time, rotation.x));
            clip.SetCurve("", typeof(Transform), property + ".x", curveX);

            AnimationCurve curveY = new AnimationCurve();
            curveY.AddKey(new Keyframe(time, rotation.y));
            clip.SetCurve("", typeof(Transform), property + ".y", curveY);

            AnimationCurve curveZ = new AnimationCurve();
            curveZ.AddKey(new Keyframe(time, rotation.z));
            clip.SetCurve("", typeof(Transform), property + ".z", curveZ);

            AnimationCurve curveW = new AnimationCurve();
            curveW.AddKey(new Keyframe(time, rotation.w));
            clip.SetCurve("", typeof(Transform), property + ".w", curveW);
        }
    }

}
