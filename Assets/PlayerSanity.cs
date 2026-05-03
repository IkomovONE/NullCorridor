using UnityEngine;


//This class is used to handle the player's sanity, which decreases over time and can be restored or lost by certain events.
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
        
        currentSanity -= drainPerSecond * Time.deltaTime;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

        if (ui != null)
            ui.UpdateSanity(currentSanity, maxSanity);
    }

    
    public void RestoreSanity(float amount)
    {
        currentSanity += amount;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

        if (ui != null)
            ui.UpdateSanity(currentSanity, maxSanity);
    }

    
    public void LoseSanity(float amount)
    {
        currentSanity -= amount;
        currentSanity = Mathf.Clamp(currentSanity, 0f, maxSanity);

        if (ui != null)
            ui.UpdateSanity(currentSanity, maxSanity);
    }
}