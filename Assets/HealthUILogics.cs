using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public TextMeshProUGUI healthText;

    void Update()
    {
        healthText.text = "Health: " + playerHealth.GetHealth();
    }
}