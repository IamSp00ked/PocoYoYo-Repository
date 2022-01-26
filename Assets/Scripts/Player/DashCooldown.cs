using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DashCooldown : MonoBehaviour
{
    public Image[] cooldownImages = new Image[3];
    private int currentIndex = 2;
    private float time= 3f;
    private float cooldownLength = 3f;
    [HideInInspector] public int dashesLeft = 3;
    
    void Start()
    {
        
    }
    void CheckCooldown()
    {
        cooldownImages[currentIndex].fillAmount = (1f / cooldownLength) * time;
    }
    public void ResetDash()
    {
        if (dashesLeft <= 2)
        {
            cooldownImages[currentIndex].fillAmount = 0f;
            currentIndex--; 
        }
        if(dashesLeft == 3)
        {
            time = 0f;
        }
        dashesLeft--;
        
        cooldownImages[currentIndex].fillAmount = 0f;
    }
    void CheckForIncrement()
    {
        if (cooldownImages[currentIndex].fillAmount == 1 && currentIndex < 2)
        {
            dashesLeft += 1;
            currentIndex++;
            time = 0f;

        }
    }

    void Update()
    {
        if (time < cooldownLength)
        {
            time += Time.deltaTime;
            if (time >= 3f)
            {
                time = 3f;
            }
        }
        CheckCooldown();
        CheckForIncrement();
    }
}
