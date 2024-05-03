using UnityEngine;
using UnityEngine.UI;
using SFB;  // Standalone File Browser namespace
using sl;  // ZED SDK namespace

public class SVOPlayer : MonoBehaviour
{
    //public Button openSVOButton;  // UI button to open file dialog
    public ZEDManager zedManager;  // ZED Manager for handling camera/SVO operations

    void Start()
    {

    }
    public void OpenFileDialog()
    {
        // Open file dialog and restrict to SVO files
        var filters = new[] { new ExtensionFilter("SVO Files", "svo") };
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
    public void ResetPlayback()
    {
        if (zedManager != null)
        {
            zedManager.Reset();
        }
    }

}
