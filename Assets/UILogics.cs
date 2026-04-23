using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image healthFill;
    public Image staminaFill;
    public Image sanityFill;

    
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
}