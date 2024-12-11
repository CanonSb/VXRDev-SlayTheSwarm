using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DayNightCycleManager : MonoBehaviour
{
    [Header("Skybox Settings")]
    [SerializeField] private Material skyboxMaterial;
    [SerializeField] private Gradient skyTintGradient;
    [SerializeField] private Gradient sunTintGradient;
    [SerializeField] private AnimationCurve exposureCurve;

    [Header("Lighting Settings")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private Gradient lightColorGradient;
    [SerializeField] private AnimationCurve lightIntensityCurve;
    [SerializeField] private float lightRotationAngle = 360f;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource dayAmbientAudio;
    [SerializeField] private AudioSource nightAmbientAudio;
    [SerializeField] private float audioTransitionDuration = 3f;

    [Header("Time Configuration")]
    [Range(0f, 1f)]
    [SerializeField] private float daytimeValue = 0.25f; // Morning (e.g., 6 AM)
    [Range(0f, 1f)]
    [SerializeField] private float nighttimeValue = 0.75f; // Evening (e.g., 6 PM)
    [Range(0f, 1f)]
    [SerializeField] private float currentTime = 0f;
    [SerializeField] private float transitionDuration = 5f; // Duration of transitions in seconds

    private Coroutine transitionCoroutine;
    private float previousTime = 0f; // Track the previous time to determine rotation direction
    private float totalRotation = 0f; // Track total rotation to maintain continuous movement

    /// <summary>
    /// Transition to the configured daytime value.
    /// </summary>
    public void SetDay()
    {
        SetTime(daytimeValue);
    }

    /// <summary>
    /// Transition to the configured nighttime value.
    /// </summary>
    public void SetNight()
    {
        SetTime(nighttimeValue);
    }

    private void SetTime(float targetTime)
    {
        targetTime = Mathf.Clamp01(targetTime);

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }

        transitionCoroutine = StartCoroutine(TransitionToTime(targetTime));
    }

    private IEnumerator TransitionToTime(float targetTime)
    {
        float startTime = currentTime;
        float elapsedTime = 0f;

        // Calculate the direction for continuous movement
        float directPath = targetTime - startTime;
        float path;

        // Always move forward (clockwise) through the day/night cycle
        if (directPath < 0)
        {
            path = 1f + directPath; // Complete the circle forward instead of going backward
        }
        else
        {
            path = directPath;
        }

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the interpolation factor with smootherstep easing
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);
            t = t * t * t * (t * (t * 6 - 15) + 10); // Smootherstep easing

            // Interpolate along the path
            currentTime = Mathf.Repeat(startTime + path * t, 1f);

            // Update the environment based on the current time
            UpdateEnvironment(currentTime);

            yield return null;
        }

        currentTime = targetTime;
        UpdateEnvironment(currentTime);
        previousTime = currentTime;

        transitionCoroutine = null;
    }

    private void UpdateEnvironment(float normalizedTime)
    {
        UpdateSkybox(normalizedTime);
        UpdateLighting(normalizedTime);
        UpdateAudio(normalizedTime);
    }

    private void UpdateSkybox(float normalizedTime)
    {
        Color skyTint = skyTintGradient.Evaluate(normalizedTime);
        skyboxMaterial.SetColor("_SkyTint", skyTint);

        Color sunTint = sunTintGradient.Evaluate(normalizedTime);
        skyboxMaterial.SetColor("_SunTint", sunTint);

        float exposure = exposureCurve.Evaluate(normalizedTime);
        skyboxMaterial.SetFloat("_Exposure", exposure);
    }

    private void UpdateLighting(float normalizedTime)
    {
        // Calculate rotation based on time of day
        float rotationAngle = normalizedTime * lightRotationAngle;

        // Convert time-based rotation to actual world space rotation
        // Adjust the rotation to start from east (-90 degrees) and rotate through south, west, and north
        float worldRotationAngle = rotationAngle - 90f;

        // Apply the rotation to the directional light
        directionalLight.transform.rotation = Quaternion.Euler(worldRotationAngle, 0f, 0f);

        // Update light color and intensity
        Color lightColor = lightColorGradient.Evaluate(normalizedTime);
        directionalLight.color = lightColor;

        float intensity = lightIntensityCurve.Evaluate(normalizedTime);
        directionalLight.intensity = intensity;
    }

    private void UpdateAudio(float normalizedTime)
    {
        if (normalizedTime > 0.25f && normalizedTime < 0.75f)
        {
            StartCoroutine(CrossfadeAudio(dayAmbientAudio, nightAmbientAudio));
        }
        else
        {
            StartCoroutine(CrossfadeAudio(nightAmbientAudio, dayAmbientAudio));
        }
    }

    private IEnumerator CrossfadeAudio(AudioSource fadeIn, AudioSource fadeOut)
    {
        float elapsedTime = 0f;

        float startVolumeIn = fadeIn.volume;
        float startVolumeOut = fadeOut.volume;

        while (elapsedTime < audioTransitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / audioTransitionDuration);

            fadeIn.volume = Mathf.Lerp(startVolumeIn, 1f, t);
            fadeOut.volume = Mathf.Lerp(startVolumeOut, 0f, t);

            yield return null;
        }

        fadeIn.volume = 1f;
        fadeOut.volume = 0f;
    }
}