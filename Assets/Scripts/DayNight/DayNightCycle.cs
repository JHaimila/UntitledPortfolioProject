using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
namespace DayNight
{
    public class DayNightCycle : MonoBehaviour
    {
        [SerializeField] private float timeMultiplier;
        [SerializeField] private float startHour;
        [SerializeField] private Light sunLight;
        [SerializeField] private float sunRiseHour;
        [SerializeField] private float sunSetHour;
        private TimeSpan sunriseTime;
        private TimeSpan sunsetTime;
        private DateTime currentTime;
        [SerializeField] private TextMeshProUGUI timeText;
    
        [SerializeField] private Color dayAmbientLight;
        [SerializeField] private Color nightAmbientLight;
        [SerializeField] private LightClass dayLight;
        [SerializeField] private LightClass nightLight;
        [SerializeField] private AnimationCurve lightChangeCurve;
        [SerializeField] private float maxSunIntensity;
        [SerializeField] private Light moonLight;
        [SerializeField] private float maxMoonIntensity;


        // Start is called before the first frame update
        void Start()
        {
            currentTime = DateTime.Now.Date + TimeSpan.FromHours(startHour);

            sunriseTime = TimeSpan.FromHours(sunRiseHour);
            sunsetTime = TimeSpan.FromHours(sunSetHour);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            UpdateTimeOfDay();
            RotateSun();
            UpdateLightSettings();
        }

        private void UpdateTimeOfDay()
        {
            currentTime = currentTime.AddSeconds(Time.deltaTime * timeMultiplier);

            // If you want to display time somewhere
            if(timeText != null)
            {
                timeText.text = currentTime.ToString("HH:mm");
            }
        }
        private void RotateSun()
        {
            float sunLightRotaion;
            if(currentTime.TimeOfDay > sunriseTime && currentTime.TimeOfDay < sunsetTime)
            {
                TimeSpan sunruseToSunsetDuration = CalcTimeDifference(sunriseTime, sunsetTime);
                TimeSpan timeSinceSunrise = CalcTimeDifference(sunriseTime, currentTime.TimeOfDay);

                double percentage = timeSinceSunrise.TotalMinutes / sunruseToSunsetDuration.TotalMinutes;

                sunLightRotaion = Mathf.Lerp(0, 180, (float)percentage);
            }
            else
            {
                TimeSpan sunsetTOSunriseDuration = CalcTimeDifference(sunsetTime, sunriseTime);
                TimeSpan timeSinceSunset = CalcTimeDifference(sunsetTime, currentTime.TimeOfDay);

                double percentage = timeSinceSunset.TotalMinutes / sunsetTOSunriseDuration.TotalMinutes;

                sunLightRotaion = Mathf.Lerp(180, 360, (float)percentage);

            }
            sunLight.transform.rotation = Quaternion.AngleAxis(sunLightRotaion, Vector3.right);
        }
        private TimeSpan CalcTimeDifference(TimeSpan fromTime, TimeSpan toTime)
        {
            TimeSpan difference = toTime - fromTime;
            
            //checks to see if the time spans more than 1 day
            if(difference.TotalSeconds < 0)
            {
                difference += TimeSpan.FromHours(24);
            }
            return difference;
        }
        private void UpdateLightSettings()
        {
            // Dot gives a value between 1 and -1 depending on how similar the two given values are. In this case if the sun is pointed directly down then we'll get 1, horizontal =0, up = 1
            float dotProduct = Vector3.Dot(sunLight.transform.forward, Vector3.down);
            if(dotProduct > 0)
            {
                // if(RenderSettings.skybox != dayLight.skybox)
                // {
                //     RenderSettings.skybox = dayLight.skybox;
                // }
                if(sunLight.shadows != LightShadows.Soft)
                {
                    sunLight.shadows = LightShadows.Soft;
                }
                // moonLight.shadows = LightShadows.None;
            }
            else if(dotProduct < 0)
            {
                // if(RenderSettings.skybox != nightLight.skybox)
                // {
                //     RenderSettings.skybox = nightLight.skybox;
                // }
                if(sunLight.shadows != LightShadows.None)
                {
                    sunLight.shadows = LightShadows.None;
                }
                // moonLight.shadows = LightShadows.Soft;
            }
            sunLight.intensity = Mathf.Lerp(0, maxSunIntensity, lightChangeCurve.Evaluate(dotProduct));
            // moonLight.intensity = Mathf.Lerp(0, maxMoonIntensity, lightChangeCurve.Evaluate(dotProduct));

            RenderSettings.ambientLight = Color.Lerp(nightLight.ambientLight, dayLight.ambientLight, lightChangeCurve.Evaluate(dotProduct));
        }
    }
}

