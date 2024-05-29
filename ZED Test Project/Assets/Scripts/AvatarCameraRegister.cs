using UnityEngine;

public class AvatarCameraRegister : MonoBehaviour
{
    private void OnEnable()
    {
        Camera avatarCamera = GetComponent<Camera>();
        if (avatarCamera != null)
        {
            CameraSwitcher.Instance.RegisterAvatarCamera(avatarCamera);
        }
    }
}
