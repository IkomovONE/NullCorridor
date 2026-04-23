using UnityEngine;

public class PlayerSanity : MonoBehaviour
{
    [Header("Sanity")]
    public float maxSanity = 100f;
    public float currentSanity = 100f;

    [Header("Drain")]
    public float drainPerSecond = 1f;

    private UIManager ui;

    void Start()
    {
        ui = FindFirstObjectByType<UIManager>();

        currentSanity = maxSanity;

        if (ui != null)
            ui.UpdateSanity(currentSanity, maxSanity);
    }

    void Update()
    {
        // Drain over time
        currentSanity -= drainPerSecond * Time.deltaTime;

        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

        // Update UI
        if (ui != null)
            ui.UpdateSanity(currentSanity, maxSanity);
    }

    // Use later for pills / medkits / safe rooms
    public void RestoreSanity(float amount)
    {
        currentSanity += amount;

        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

        if (ui != null)
            ui.UpdateSanity(currentSanity, maxSanity);
    }

    // Optional if enemy events damage sanity later
    public void LoseSanity(float amount)
    {
        currentSanity -= amount;

        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

        if (ui != null)
            ui.UpdateSanity(currentSanity, maxSanity);
    }
}