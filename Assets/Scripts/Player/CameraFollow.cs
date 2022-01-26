using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Public Components
    public Transform player;
    public Camera currentCamera;
    // Private Components
    private Rigidbody rb;
    // Public variables
    [Range(-90f, 0f)]
    public float minYangle;
    [Range(-90f, 0f)]
    public float maxYangle;
    public float sensitivity;
    public float rotationTime;
    // Private variables
    [HideInInspector] public Vector3 rotation;
    private CameraRotate cR;
    private Vector3 mouseWorldPosition;
    private float angle;
    float rayLength;
    public float deadzoneRange;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cR = currentCamera.GetComponent<CameraRotate>();
    }

    void Update()
    {


        // Constantly reads mouse movement to rotate the objec

        // Use when limitations will be set
        //rotation.y = Mathf.Clamp(rotation.y, minYangle, maxYangle);
        //player.LookAt(currentCamera.ScreenToWorldPoint(Input.mousePosition));
        // Applies rotation
        /*
        if (cR.rotateActive)
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(-rotation.y, rotation.x, 0f), rotationTime);
        */
        /*
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);
        angle = AngleBetweenPoints(transform.position, mouseWorldPosition);
        player.rotation = Quaternion.Euler(new Vector3(0f, -angle, 0f));
        */
        /*
        mouseWorldPosition = currentCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z-10f));
        player.LookAt(new Vector3(mouseWorldPosition.x,player.position.y,mouseWorldPosition.z));
        Debug.Log(mouseWorldPosition);
        */

        mouseWorldPosition = currentCamera.ScreenToViewportPoint(Input.mousePosition);
        Ray cameraRay = currentCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            if(Vector3.Distance(transform.position,pointToLook) >deadzoneRange)   
            player.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        //Debug.Log(cameraRay.GetPoint(rayLength));
    }

    private void FixedUpdate()
    {
        // Makes the object follow the player
        rb.position = Vector3.Lerp(rb.position, player.position, 0.1f);
    }
    float AngleBetweenPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}