using UnityEngine;

[ExecuteAlways]
public class LightningManager : MonoBehaviour
{
    // Refs
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightningPreset Preset;
    
    // Vars
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    private void Update()
    {
        if (Preset == null)
            return;
        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24; // Convert to 0-24 format
            UpdateLightning(TimeOfDay / 24);
        }
        else
        {
            UpdateLightning(TimeOfDay / 24);
        }
    }

    private void UpdateLightning(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null )
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360) - 90.0f, 170.0f, 0.0f));
        }
    }

    private void OnValidate()
    {
        if (DirectionalLight != null)
            return;
        
        if (RenderSettings.sun != null)
            DirectionalLight = RenderSettings.sun;
        else
        {
            Light[] lights = FindObjectsByType<Light>(FindObjectsSortMode.None);
            foreach (Light light in lights){
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}
