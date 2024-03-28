using UnityEngine;
using System.Collections.Generic;

public class AnimationRecorder : MonoBehaviour
{
    public KeyCode startRecordingKey = KeyCode.R; // Key to start recording
    public KeyCode stopRecordingKey = KeyCode.S; // Key to stop recording

    private List<Animator> animatorsToRecord = new List<Animator>(); // List to store Animator components of all models
    private AnimationClip recordedClip; // The recorded animation clip
    private bool isRecording = false; // Flag to indicate if recording is in progress

    // Update is called once per frame
    void Update()
    {
        // Check if the recording key is pressed
        if (Input.GetKeyDown(startRecordingKey))
        {
            StartRecording();
        }

        // Check if the stop key is pressed
        if (Input.GetKeyDown(stopRecordingKey))
        {
            StopRecording();
        }

        // Record animation if recording flag is true
        if (isRecording)
        {
            RecordAnimation();
        }
    }

    // Method to start recording animation
    private void StartRecording()
    {
        // Find all GameObjects with Animator components
        Animator[] allAnimators = FindObjectsOfType<Animator>();
        foreach (Animator animator in allAnimators)
        {
            animatorsToRecord.Add(animator);
        }

        // Create a new animation clip
        recordedClip = new AnimationClip();
        recordedClip.name = "RecordedClip_" + Time.time;

        Debug.Log("Recording started...");
        isRecording = true;
    }

    // Method to stop recording animation
    private void StopRecording()
    {
        if (isRecording)
        {
            isRecording = false;
            Debug.Log("Recording stopped.");

            // Save the recorded animation clip as an asset
            SaveAnimationClip();
        }
    }

    // Method to record animation
    private void RecordAnimation()
    {
        if (animatorsToRecord.Count == 0)
        {
            Debug.LogError("No animators found to record.");
            return;
        }

        // Get the current frame count
        int frameCount = GetMaxFrameCount();

        // Capture keyframe data for each frame
        for (int i = 0; i <= frameCount; i++)
        {
            float time = i / 60f; // Assuming 60 frames per second
            foreach (Animator animator in animatorsToRecord)
            {
                animator.Update(time);
            }
            // Add keyframe data for each animator's properties
            // Adjust this part based on what properties you want to record
        }
    }

    // Method to get the maximum frame count among all animators
    private int GetMaxFrameCount()
    {
        int maxFrameCount = 0;
        foreach (Animator animator in animatorsToRecord)
        {
            if (animator != null)
            {
                int frameCount = Mathf.FloorToInt(animator.GetCurrentAnimatorStateInfo(0).length * animator.GetCurrentAnimatorStateInfo(0).speed);
                if (frameCount > maxFrameCount)
                {
                    maxFrameCount = frameCount;
                }
            }
        }
        return maxFrameCount;
    }

    // Method to save recorded animation clip as an asset
    private void SaveAnimationClip()
    {
        if (recordedClip == null)
        {
            Debug.LogError("No animation clip recorded.");
            return;
        }

        // Save the recorded clip as an asset
        string path = "Assets/RecordedAnimations/" + recordedClip.name + ".anim";
        UnityEditor.AssetDatabase.CreateAsset(recordedClip, path);
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log("Animation clip saved: " + path);
    }
}
