using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MicrophoneDropdown : MonoBehaviour
{
    public TMP_Dropdown microphoneDropdown;
    public TMP_Text selectedMicrophoneText; // A Text component to display the selected microphone name
    public Button assignMicrophoneButton; // Button to assign microphone
    public GameObject targetGameObject; // GameObject to assign the microphone name to
    private string[] microphones; // Array of available microphones
    private string selectedMicrophone; // Currently selected microphone

    void Start()
    {
        // Get the list of available microphones
        microphones = Microphone.devices;

        // Clear existing options in the dropdown
        microphoneDropdown.ClearOptions();

        if (microphones.Length > 0)
        {
            // Populate dropdown with microphone names
            microphoneDropdown.AddOptions(new System.Collections.Generic.List<string>(microphones));

            // Set the default microphone
            microphoneDropdown.value = 0;

            // Set the initial selected microphone and update the display
            selectedMicrophone = microphones[0];
            UpdateSelectedMicrophoneText();
        }

        // Add listener to handle when the dropdown value changes
        microphoneDropdown.onValueChanged.AddListener(OnMicrophoneChanged);

        // Add listener to handle button click to assign the selected microphone
        if (assignMicrophoneButton != null)
        {
            assignMicrophoneButton.onClick.AddListener(AssignMicrophoneToGameObject);
        }
    }

    private void OnMicrophoneChanged(int index)
    {
        // Update the selected microphone when dropdown changes
        selectedMicrophone = microphones[index];
        UpdateSelectedMicrophoneText();
    }

    private void UpdateSelectedMicrophoneText()
    {
        if (selectedMicrophoneText != null)
        {
            selectedMicrophoneText.text = "Selected Microphone: " + selectedMicrophone;
        }
    }

    private void AssignMicrophoneToGameObject()
    {
        if (targetGameObject != null)
        {
            // Assign the selected microphone name to the GameObject
            targetGameObject.name = selectedMicrophone;

            Debug.Log("Assigned " + selectedMicrophone + " to GameObject " + targetGameObject.name);
        }
        else
        {
            Debug.LogWarning("Target GameObject is not assigned.");
        }
    }
}
