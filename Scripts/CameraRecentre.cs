using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraRecentre : MonoBehaviour
{
    private CinemachineFreeLook _camera;
    // Start is called before the first frame update
    void Start()
    {
        _camera = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("CameraRecentre") == 1)
        {
            _camera.m_RecenterToTargetHeading.m_enabled = true;
        }
        else
        {
            _camera.m_RecenterToTargetHeading.m_enabled = false;
        }
    }
}
