using UnityEngine;
using System.Collections.Generic;

public class AnimationRecorder : MonoBehaviour
{
    public string modelTag = "RecordableModel"; // Tag for the model to record
    public Animator animator; // Animator component of the model to record animations from

    private List<AnimationClip> recordedClips = new List<AnimationClip>(); // List to store recorded animation clips
    private bool isRecording = false; // Flag to indicate if recording is in progress

    // Start is called before the first frame update
    void Start()
    {
        // Find the GameObject with the specified tag
        GameObject modelObject = GameObject.FindGameObjectWithTag(modelTag);
        if (modelObject != null)
        {
            // Get the Animator component attached to the model
            animator = modelObject.GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("No GameObject found with tag: " + modelTag);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if recording is in progress
        if (isRecording && animator != null)
        {
            // Record animation if recording flag is true
            RecordAnimation();
        }
    }

    // Method to start recording animation
    public void StartRecording()
    {
        if (animator != null)
        {
            isRecording = true;
            recordedClips.Clear(); // Clear any previous recorded clips
            Debug.Log("Recording started...");
        }
        else
        {
            Debug.LogError("Animator component not assigned!");
        }
    }

    // Method to stop recording animation
    public void StopRecording()
    {
        isRecording = false;
        Debug.Log("Recording stopped.");

        // Export recorded animation clips
        ExportAnimationClips();
    }

    // Method to record animation
    private void RecordAnimation()
    {
        // Create a new animation clip
        AnimationClip newClip = new AnimationClip();
        newClip.name = "RecordedClip_" + Time.time;

        // Get the current frame count
        int frameCount = Mathf.FloorToInt(animator.GetCurrentAnimatorStateInfo(0).length * animator.GetCurrentAnimatorStateInfo(0).speed);

        // Capture keyframe data for each frame
        for (int i = 0; i <= frameCount; i++)
        {
            float time = i / animator.GetCurrentAnimatorStateInfo(0).speed;
            animator.Update(time);

            // Capture keyframes for position, rotation, and scale
            Keyframe[] posXFrames = new Keyframe[] { new Keyframe(time, transform.localPosition.x) };
            Keyframe[] posYFrames = new Keyframe[] { new Keyframe(time, transform.localPosition.y) };
            Keyframe[] posZFrames = new Keyframe[] { new Keyframe(time, transform.localPosition.z) };
            Keyframe[] rotXFrames = new Keyframe[] { new Keyframe(time, transform.localRotation.eulerAngles.x) };
            Keyframe[] rotYFrames = new Keyframe[] { new Keyframe(time, transform.localRotation.eulerAngles.y) };
            Keyframe[] rotZFrames = new Keyframe[] { new Keyframe(time, transform.localRotation.eulerAngles.z) };
            Keyframe[] scaleXFrames = new Keyframe[] { new Keyframe(time, transform.localScale.x) };
            Keyframe[] scaleYFrames = new Keyframe[] { new Keyframe(time, transform.localScale.y) };
            Keyframe[] scaleZFrames = new Keyframe[] { new Keyframe(time, transform.localScale.z) };

            AnimationCurve posXCurve = new AnimationCurve(posXFrames);
            AnimationCurve posYCurve = new AnimationCurve(posYFrames);
            AnimationCurve posZCurve = new AnimationCurve(posZFrames);
            AnimationCurve rotXCurve = new AnimationCurve(rotXFrames);
            AnimationCurve rotYCurve = new AnimationCurve(rotYFrames);
            AnimationCurve rotZCurve = new AnimationCurve(rotZFrames);
            AnimationCurve scaleXCurve = new AnimationCurve(scaleXFrames);
            AnimationCurve scaleYCurve = new AnimationCurve(scaleYFrames);
            AnimationCurve scaleZCurve = new AnimationCurve(scaleZFrames);

            // Set the curves for position, rotation, and scale
            newClip.SetCurve("", typeof(Transform), "localPosition.x", posXCurve);
            newClip.SetCurve("", typeof(Transform), "localPosition.y", posYCurve);
            newClip.SetCurve("", typeof(Transform), "localPosition.z", posZCurve);
            newClip.SetCurve("", typeof(Transform), "localRotation.x", rotXCurve);
            newClip.SetCurve("", typeof(Transform), "localRotation.y", rotYCurve);
            newClip.SetCurve("", typeof(Transform), "localRotation.z", rotZCurve);
            newClip.SetCurve("", typeof(Transform), "localScale.x", scaleXCurve);
            newClip.SetCurve("", typeof(Transform), "localScale.y", scaleYCurve);
            newClip.SetCurve("", typeof(Transform), "localScale.z", scaleZCurve);
        }

        // Add the new clip to the list of recorded clips
        recordedClips.Add(newClip);
    }

    // Method to export recorded animation clips
    private void ExportAnimationClips()
    {
        if (recordedClips.Count == 0)
        {
            Debug.LogError("No animation clips recorded.");
            return;
        }

        // Save the recorded animation clips
        foreach (AnimationClip clip in recordedClips)
        {
            string path = Application.persistentDataPath + "/RecordedAnimations/" + clip.name + ".anim";
            SaveAnimationClip(clip, path);
            Debug.Log("Animation clip saved to: " + path);
        }
    }

    // Method to save animation clip to file
    private void SaveAnimationClip(AnimationClip clip, string path)
    {
        var data = clip;
        using (var writer = new System.IO.FileStream(path, System.IO.FileMode.Create))
        {
            var bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            bf.Serialize(writer, data);
        }
    }
}
