using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour
{
    public Camera currentCamera;
    public Collider[] playerCollider;
    public LayerMask layerMask;
    public bool playerIsInside = true;
    Collider[] playerInside;
    Collider[] playerOutside;
    public GameObject display;
    private bool hasInteracted;
    private CameraRotate cR;

    void Start()
    {
        display.SetActive(false);
        cR = currentCamera.GetComponent<CameraRotate>();
    }


    void Update()
    {
        // Check if you can activate dialogue
        if (playerIsInside)
        {
            Debug.Log("NICKLAS IS AWARE OF YOUR PRECENCE");
            // Enable all text boxes that are needed
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Interactions here
                Debug.Log("NICKLAS IS AWARE OF YOUR ¨e¨ PRESSING ABILITIES");
                hasInteracted = !hasInteracted;
                ShowDisplay();
            }
        }

    }
    private void FixedUpdate()
    {
        // Check for the player near NPC
        playerInside = Physics.OverlapSphere(transform.position, 2f, layerMask);
        foreach (Collider collider in playerInside)
        {
            playerIsInside = true;
        }
        playerOutside = playerCollider.Except(playerInside).ToArray();
        foreach (Collider collider in playerOutside)
        {
            {
                if (hasInteracted)
                    ShowDisplay();
                playerIsInside = false;
                hasInteracted = false;
            }
        }
    }


    private void ShowDisplay()
    {
        if (hasInteracted)
        {
            playerCollider[0].gameObject.GetComponent<PlayerScript>().allowedControl = false;
            cR.SetCamera(false);
            display.SetActive(true);
        }
        else
        {
            playerCollider[0].gameObject.GetComponent<PlayerScript>().allowedControl = true;
            cR.SetCamera(true);
            display.SetActive(false);
        }
    }
}
