using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{

    [SerializeField] private Light directionalLight;
    [SerializeField] private LightingPreset preset;

    [SerializeField, Range(0, 24)] private float worldTime;


    private void Update()
    {
        if (preset == null)
            return;

        if (Application.isPlaying)
        {
            worldTime += Time.deltaTime;
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
