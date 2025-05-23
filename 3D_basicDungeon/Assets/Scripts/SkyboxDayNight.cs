using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class SkyboxDayNight : MonoBehaviour
{
    public Material sunriseMaterial; // SkyhighFluffycloudField4k.mat
    public Material sunsetMaterial; // CloudedSunGlow4k.mat
    [Range(0, 1)] public float blend; // 0: sunrise, 1: sunset(�� ��)

    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
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
        // ó�� skybox�� SkyhighFluffycloudField4k ���� ����
        RenderSettings.skybox = Instantiate(sunriseMaterial);

        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        // �ð��� ���� �ΰ��� blend �� �ڵ� ���
        if (time <= 0.5f)
            blend = Mathf.InverseLerp(0f, 0.5f, time);

        else
            blend = Mathf.InverseLerp(0.5f, 1f, time);

        // skybox ȥ��
        RenderSettings.skybox.Lerp(sunriseMaterial, sunsetMaterial, blend); // �� ������ blend ������ ȥ��
        DynamicGI.UpdateEnvironment(); // GI ������Ʈ
    }

    void UpdateLighting(Light lightSource, Gradient colorGradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        // �ð� - �¾��� ���� �ð�(12:00 PM) = �¾��� ȸ�� ����
        // �¾��� ȸ�� ���� * noon �� ���� ���� ���ϸ� �ӵ��� ������
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 0.7f;
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
