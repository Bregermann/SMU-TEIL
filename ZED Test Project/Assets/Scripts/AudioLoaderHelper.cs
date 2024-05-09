using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLoaderHelper : MonoBehaviour
{
    public SVOPlayer doItNow;
    public int buttonIndexPlease;
public void ButtonIndexMeNow()
    {
        doItNow.buttonIndex = buttonIndexPlease;
    }
}
