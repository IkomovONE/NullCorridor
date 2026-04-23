using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class UIManager : MonoBehaviour
{
    public Image healthFill;
    public Image staminaFill;
    public Image sanityFill;

    Coroutine hpGlow;
    Coroutine stmGlow;
    Coroutine sanGlow;

    
    private float smoothSpeed = 4f;
    private float targetHealth = 2f;
    

    void Update()

    {

        healthFill.fillAmount =

            Mathf.Lerp(healthFill.fillAmount, targetHealth, Time.deltaTime * smoothSpeed);

    }

    public void UpdateHealth(float current, float max)

    {

        targetHealth = current / max;

    }

    public void UpdateStamina(float current, float max)
    {
        staminaFill.fillAmount = current / max;
    }

    public void UpdateSanity(float current, float max)
    {
        sanityFill.fillAmount = current / max;
    }

    public void GlowHealth()
    {
        if (hpGlow != null) StopCoroutine(hpGlow);
        hpGlow = StartCoroutine(GlowBar(healthFill, new Color(1f, 0.4f, 0.4f)));
    }

    public void GlowStamina()
    {
        if (stmGlow != null) StopCoroutine(stmGlow);
        stmGlow = StartCoroutine(GlowBar(staminaFill, new Color(0.4f, 0.7f, 1f)));
    }

    public void GlowSanity()
    {
        if (sanGlow != null) StopCoroutine(sanGlow);
        sanGlow = StartCoroutine(GlowBar(sanityFill, new Color(0.8f, 0.4f, 1f)));
    }


    IEnumerator GlowBar(UnityEngine.UI.Image bar, Color glowColor)
    {
        Color original = bar.color;

        bar.color = glowColor;

        yield return new WaitForSeconds(0.18f);

        bar.color = original;
    }
}