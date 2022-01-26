using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{
    public Text timeText;
    public Text daysText;
    public GameObject lights;
    public int privateTimeScale = 1;
    //public LightingPreset preset;

    // Used to update ingame seconds for calculations though they aren't shown
    private float timeDelta = 0f;
    // Used to update ingame hours and are being displayed as minutes
    [SerializeField] private float minutes = 0;
    private int startMinutes = 0;
    private float totalMinutes = 0;
    // Displaying ingame hours
    [SerializeField] private int hours = 6;
    // Used to display days
    private int days = 1;

    private void Start()
    {
        minutes += startMinutes;
        UpdateClock();
        UpdateDays();
    }

    private void Update()
    {
        //Debug.Log(totalMinutes + timeDelta);
        UpdateMinutes();
        UpdateClock();
        // Constantly rotates the light around 360 degrees 
        //RotateLight();
        RotateLight((totalMinutes * privateTimeScale) / 1440f);
    }
    /// <summary>
    /// Updates Minutes ingame
    /// </summary>
    private void UpdateMinutes()
    {
        totalMinutes += Time.deltaTime;
        timeDelta += Time.deltaTime;
        minutes = timeDelta * privateTimeScale;
        if (minutes >= 60)
        {
            minutes = 0;
            timeDelta = 0;
            UpdateHours();
        }
    }
    /// <summary>
    /// Update hours ingame 
    /// </summary>
    /// <param name="tempMinutes"> Current ingame minutes </param>
    private void UpdateHours()
    {
        hours++;
        //minutes = 0;
        if (hours == 24)
        {
            days++;
            hours = 0;
            UpdateDays();
        }
    }
    /// <summary>
    /// Updates the ingame clock with minutes and hours
    /// </summary>
    private void UpdateClock()
    {
        timeText.text = LeadingZero(hours) + ":" + LeadingZero((int)minutes);
    }
    // Updates ingame days display
    private void UpdateDays()
    {
        daysText.text = "Day " + days;
    }
    // Sets the layout of a digital clock
    private string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
    /// <summary>
    /// Rotates the lightsource to the set amount of degrees
    /// </summary>
    /// <param name="inpDegrees"></param>

    private void RotateLight(float timeOfDay)
    {
       /* if (preset != null)
        {
            RenderSettings.ambientLight = preset.AmbientColor.Evaluate(timeOfDay);
            lights.GetComponent<Light>().color = preset.DirectionalColor.Evaluate(timeOfDay);
        }*/
        lights.transform.localRotation = Quaternion.Euler(new Vector3((timeOfDay * 360f), 0f, 0f));
        if (lights.transform.rotation.x >= 359.8f)
        {
            totalMinutes = 0;
        }
    }
}

