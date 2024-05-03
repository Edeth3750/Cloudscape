using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{

    []
    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;

    [SerializeField, Range(0, 24)] private float worldTime;

    //state that alters the rate of time 
    private enum timeRate
    {
        seconds,
        minutes,
        hours
    }

    [SerializeField] private timeRate rateOfTime;


    //alter Time.deltaTime to change timelapse of day/night
    private float scaledTime;


    private void Update()
    {
        if (preset == null)
            return;

        if (Application.isPlaying)
        {
            switch (rateOfTime)
            {
                case (timeRate.hours):
                    scaledTime = (Time.deltaTime / 60) / 60;
                    break;


                case (timeRate.minutes):
                    scaledTime = (Time.deltaTime / 60);
                    break;

                case (timeRate.seconds):
                    scaledTime = Time.deltaTime;
                    break;
            }

            worldTime += scaledTime;
            worldTime %= 24; //Clamp between 0-24

            UpdateLighting(worldTime/24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.ambientColour.Evaluate(timePercent);
        RenderSettings.fogColor = preset.fogColour.Evaluate(timePercent);

        if (directionalLight != null)
        {
            directionalLight.color = preset.directionalColour.Evaluate(timePercent);
            directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(timePercent * 360 - 90f, 170f, 0));
        }
    }


    private void OnValidate()
    {


        if (directionalLight != null)
            return;

        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach(Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    directionalLight = light;
                }
            }

        } 

    }

}
