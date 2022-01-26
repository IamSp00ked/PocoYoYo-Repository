using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float newFov;
    private Camera currentCamera;
    private int zoom = 0;
    private float prevFov = 60f;

    private void Start()
    {
        currentCamera = GetComponent<Camera>();
    }
    private void Update()
    {
        if (currentCamera.GetComponent<CameraRotate>().rotateActive)
        {
            if (Input.GetMouseButtonDown(0))
                zoom = 1;
            else if (Input.GetMouseButtonUp(0))
                zoom = 2;

            if (zoom == 1)
                Zoom(newFov);
            if (zoom == 2)
                Zoom(prevFov);

        }
    }
    public void Zoom(float inpFov)
    {
        currentCamera.fieldOfView = Mathf.Lerp(currentCamera.fieldOfView, inpFov, 0.1f);
    }
}

