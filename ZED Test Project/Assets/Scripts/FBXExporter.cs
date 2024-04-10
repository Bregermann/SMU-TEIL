using UnityEngine;
using System;
using Assimp;
using Assimp.Configs;

public class FBXExporter : MonoBehaviour
{
    public string filePath = "Assets/ExportedAnimation.fbx";
    public GameObject[] animatedObjects; // List of GameObjects containing animated data

    void Start()
    {
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
        ExportToFBX();
        }
    }
    void ExportToFBX()
    {
        AssimpContext context = new AssimpContext();
        Scene scene = new Scene();

        // Configure exporter settings
        ExportFormat format = ExportFormat.FBX7200Binary; // Choose FBX format (e.g., FBX7200Binary for binary format)
        ExportDataBlob exportBlob = new ExportDataBlob();

        // Iterate through animated objects
        foreach (GameObject obj in animatedObjects)
        {
            // Create a node for each animated object
            Node node = new Node(obj.name);
            node.Transform = AssimpUnity.Convert.FromUnityTransform(obj.transform);

            // Convert mesh data
            Assimp.Mesh mesh = AssimpUnity.Convert.FromUnityMesh(obj.GetComponent<MeshFilter>().sharedMesh);

            // Add mesh to node
            node.MeshIndices.Add(scene.MeshCount);
            scene.Meshes.Add(mesh);

            // Add node to the scene
            scene.RootNode.Children.Add(node);
        }

        // Export animation data (if available)
        foreach (GameObject obj in animatedObjects)
        {
            Animation animation = obj.GetComponent<Animation>();
            if (animation != null)
            {
                // Convert animation data
                Assimp.Animation anim = AssimpUnity.Convert.FromUnityAnimation(animation);

                // Add animation to the scene
                scene.Animations.Add(anim);
            }
        }

        // Export scene to FBX file
        context.ExportFile(scene, filePath, format);

        Debug.Log("Exported animation to FBX: " + filePath);
    }
}
