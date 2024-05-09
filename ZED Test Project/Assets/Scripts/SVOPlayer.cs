using UnityEngine;
using UnityEngine.UI;
using SFB;  // Standalone File Browser namespace
using sl;  // ZED SDK namespace
using System.Windows.Forms;
using UnityEngine.UIElements;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SVOPlayer : MonoBehaviour
{
    public UnityEngine.UI.Button openSVOButton;  // UI button to open file dialog
    public ZEDManager zedManager;  // ZED Manager for handling camera/SVO operations
    public UnityEngine.UI.Button playSceneButton;
    public UnityEngine.UI.Button[] audioButtons;  // Array of buttons for selecting audio files
    public AudioSource[] audioSources;  // Array of corresponding AudioSources
    public int buttonIndex;
    public GameObject audioButtonsUIObject;
    public GameObject zedRig;
    public Material skyboxMaterial;

    void Start()
    {
        DynamicGI.UpdateEnvironment();
        audioButtonsUIObject.SetActive(false);
        zedRig.SetActive(true);
        openSVOButton.onClick.AddListener(OpenSelectedSVO);
        playSceneButton.onClick.AddListener(PlayScene);
        // Ensure there's a matching AudioSource for each button
        if (audioButtons.Length != audioSources.Length)
        {
            Debug.LogError("Mismatch between the number of buttons and AudioSources.");
            return;
        }

        // Add listeners to the audio buttons for loading audio files
        for (int i = 0; i < audioButtons.Length; i++)
        {
            int index = i;  // Capture index for lambda
            audioButtons[i].onClick.AddListener(() => OpenAudioFileDialog(index));
        }
    }

    void OpenSelectedSVO()
    {
        OpenFileDialog();
        zedRig.SetActive(false);
    }
    void PlayScene()
    {
        zedRig.SetActive(true);
        ResetPlayback();
        PlayAllSources();
        RenderSettings.skybox = skyboxMaterial;
        DynamicGI.UpdateEnvironment();
        audioButtonsUIObject.SetActive(false);

    }
    public void OpenFileDialog()
    {
        // Open file dialog and restrict to SVO files
        var filters = new[] { new ExtensionFilter("SVO Files", "svo", "svo2") };
        var paths = StandaloneFileBrowser.OpenFilePanel("Select an SVO File", "", filters, false);

        if (paths.Length > 0)
        {
            string svoPath = paths[0];  // Get the selected file path
            Debug.Log("Selected SVO file: " + svoPath);

            if (zedManager != null)
            {
                zedManager.svoInputFileName = svoPath;
            }
        }
    }
    public void OpenAudioFileDialog(int buttonindex)
    {
        var filters = new[] { new ExtensionFilter("Audio Files", "wav", "mp3", "ogg") };
        var paths = StandaloneFileBrowser.OpenFilePanel("Select an Audio File", "", filters, false);

        if (paths.Length > 0)
        {
            string audioFilePath = paths[0];
            StartCoroutine(LoadAudioClip(audioFilePath, buttonIndex));  // Load the audio asynchronously
        }
    }
    IEnumerator LoadAudioClip(string path, int buttonIndex)
    {
        WWW www = new WWW("file://" + path);  // Unity's WWW class for loading local files
        yield return www;

        if (www.error == null)
        {
            AudioClip clip = www.GetAudioClip(false, true);  // Convert to AudioClip
            audioSources[buttonIndex].clip = clip;  // Assign the clip to the correct AudioSource
            Debug.Log($"Audio clip loaded for button {buttonIndex}.");
        }
        else
        {
            Debug.LogError("Error loading audio: " + www.error);
        }
    }
    // Function to play all AudioSources in the scene
    public void PlayAllSources()
    {
        // Find all GameObjects with an AudioSource component
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();

        // Iterate through each AudioSource and play it
        foreach (AudioSource source in allAudioSources)
        {
            if (!source.isPlaying)  // Play only if it's not already playing
            {
                source.Play();
            }
        }
    }
    public void ResetPlayback()
    {
        if (zedManager != null)
        {
            zedManager.Reset();
        }
    }
    public void OpenAudioButtons()
    {
        audioButtonsUIObject.SetActive(true);
    }

}
