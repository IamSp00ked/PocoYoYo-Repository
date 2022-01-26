using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public bool rotateActive = true;
    // Public Objects/components
    public GameObject camTarget;

    private GameObject player;
    // Public variables
    public float rotTime;
    private void Start()
    {
        // Hides and locks the cursor to prevent travel outside the game window
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
    }
    void Update()
    {
        // Rotates the camera to the rotation the camera target is in
        /*
        if (rotateActive)
            transform.rotation = Quaternion.Lerp(transform.rotation, camTarget.transform.rotation, rotTime);
        */
    }

    public void SetCamera(bool inpActive)
    {
        if (!inpActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            rotateActive = false;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            rotateActive = true;
        }
    }
}