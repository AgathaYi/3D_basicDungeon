using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class SkyboxDayNight : MonoBehaviour
{
    public Material sunriseMaterial; // SkyhighFluffycloudField4k.mat
    public Material sunsetMaterial; // CloudedSunGlow4k.mat
    [Range(0, 1)] public float blend; // 0: sunrise, 1: sunset(해 짐)

    public float time;
    public float fullDayLength;
    public float startTime = 0.4f; // 0.5f = 12:00 PM. 0.4f = 10:00 AM
    private float timeRate;
    public Vector3 noon; // Vector (90, 0, 0) = 12:00 PM

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionIntensityMultiplier;

    void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.skybox.Lerp(sunriseMaterial, sunsetMaterial, blend); // 두 재질을 blend 비율로 혼합
        DynamicGI.UpdateEnvironment(); // GI 업데이트
    }

    void UpdateLighting(Light lightSource, Gradient colorGradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4.0f;
        lightSource.color = colorGradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}
