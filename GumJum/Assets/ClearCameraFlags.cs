using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCameraFlags : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
