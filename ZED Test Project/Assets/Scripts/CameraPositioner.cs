using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositioner : MonoBehaviour
{
    public Transform leftEyeTrans;
    public Transform rightEyeTrans;
    public GameObject leftEye;
    public GameObject rightEye;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftEye.transform.position = leftEyeTrans.position; 
        rightEye.transform.position = rightEyeTrans.position;
    }
}
