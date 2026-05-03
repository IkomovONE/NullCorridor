using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;


//This class is used to create a flickering effect for the lights in the game, adding to the atmosphere and tension.
public class FlickerLight : MonoBehaviour
{
    public Light2D light2D;
    public float minIntensity = 0.15f;
    public float maxIntensity = 0.55f;
    public float flickerChance = 0.15f;
    public float checkInterval = 0.12f;


    void Start()
    {
        if (light2D == null)
            light2D = GetComponent<Light2D>();

        StartCoroutine(FlickerRoutine());
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            if (Random.value < flickerChance)
            {
                yield return StartCoroutine(DoFlickerBurst());
            }
            else
            {
                light2D.intensity = maxIntensity;
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }

    IEnumerator DoFlickerBurst()
    {
        int flashes = Random.Range(2, 6);

        for (int i = 0; i < flashes; i++)
        {
            light2D.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(Random.Range(0.03f, 0.09f));
        }

        light2D.intensity = maxIntensity;
    }
}