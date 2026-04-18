using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    private Light2D light2D;
    private float baseIntensity;
    private float timer;
    private float BlackoutTimer;

    private float BlackoutTime;

    

    void Start()
    {
        light2D = GetComponent<Light2D>();
        baseIntensity = light2D.intensity;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        BlackoutTimer -= Time.deltaTime;

        // If currently in blackout
        if (BlackoutTime > 0f)
        {
            BlackoutTime -= Time.deltaTime;
            light2D.intensity = 0f;
            Debug.Log("Blackout Time: " + BlackoutTime);

            if (BlackoutTime <= 0f)
            {
                light2D.intensity = baseIntensity;
                BlackoutTimer = Random.Range(30f, 120f);
                Debug.Log("Blackout Ended. Next blackout in: " + BlackoutTimer);
            }

            return;
        }

        // Normal flicker
        if (timer <= 0f)
        {
            light2D.intensity = baseIntensity + Random.Range(-0.30f, 0.08f);
            timer = Random.Range(0.03f, 0.12f);

            if (BlackoutTimer <= 3f)
            {
                light2D.intensity = baseIntensity + Random.Range(-2f, -0.9f);
                
            }
        }

        // Start blackout
        if (BlackoutTimer <= 0f)
        {
            BlackoutTime = Random.Range(1f, 3f);
        }
    }
}