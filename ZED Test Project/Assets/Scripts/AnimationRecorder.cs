using UnityEngine;
using UnityEditor;

public class AnimationRecorder : MonoBehaviour
{
    public string modelTag = "RecordMe"; // Tag for the model to record
    public AnimationClip animationClip; // The animation clip to record

    private Animator animator; // Reference to the Animator component of the model

    public bool isAnimationRecording;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            isAnimationRecording = true;
        }
        // Check if the animator is valid and the animation clip is assigned
        if (animator != null && animationClip != null && isAnimationRecording)
        {
            // Record animation
            RecordAnimation();
        }
    }
    public void FindAvatar()
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
    public void RecordAnimation()
    {
        // Sample the animation state
        animator.Update(0f);

        // Capture the animation state
        AnimationMode.StartAnimationMode();
        AnimationMode.BeginSampling();
        AnimationMode.SampleAnimationClip(animator.gameObject, animationClip, Time.time);
        AnimationMode.EndSampling();
        AnimationMode.StopAnimationMode();
    }

    public void ExportAnimationClip()
    {
        // Check if the animation clip is assigned
        if (animationClip == null)
        {
            Debug.LogError("Animation clip is not assigned.");
            return;
        }

        // Save the animation clip
        string path = EditorUtility.SaveFilePanel("Save Animation", "", "NewAnimation", "anim");
        if (!string.IsNullOrEmpty(path))
        {
            // Ensure the path has the .anim extension
            if (!path.EndsWith(".anim"))
            {
                path += ".anim";
            }

            // Create an AnimationClip and copy the recorded animation data to it
            AnimationClip newClip = new AnimationClip();
            EditorUtility.CopySerialized(animationClip, newClip);

            // Save the new animation clip to the specified path
            AssetDatabase.CreateAsset(newClip, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Animation clip saved to: " + path);
        }
    }
}