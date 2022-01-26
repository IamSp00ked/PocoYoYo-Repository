using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Text timerText;

    public float timeLeft = 180f;
    private int seconds;
    public bool takeTime = true;
    private string timeIsUp = "Your time is up";
    void Start()
    {
        timerText = GetComponent<Text>();
    }

    void Update()
    {
        TakeTime();
        
    }
    void TakeTime()
    {
        if (takeTime && timeLeft > 30f)
        {
            timeLeft -= Time.deltaTime;
            seconds = (int)(timeLeft);
            timerText.text = seconds.ToString();
        }
        else if(takeTime && timeLeft < 30f)
        {
            timeLeft -= Time.deltaTime;
            timeLeft = Mathf.Round(timeLeft * 100f) * 0.01f;
            timerText.text = timeLeft.ToString();
        }
        if (timeLeft <= 0)
        {
            Debug.Log("Time is up");
            takeTime = false;
            timerText.text = timeIsUp;
        }
        

    }
}
