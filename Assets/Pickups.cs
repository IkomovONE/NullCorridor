using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public enum PickupType
    {
        Health,
        Stamina,
        Sanity,
        Ammo
    }

    public AudioClip medkitSound;
    public AudioClip drinkSound;
    public AudioClip pillSound;

    public AudioClip ammoSound;

    public PickupType pickupType;
    public float amount = 25f;

    private UIManager ui;

    void Start()
    {
        ui = FindFirstObjectByType<UIManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth hp = other.GetComponent<PlayerHealth>();
        PlayerMovement movement = other.GetComponent<PlayerMovement>();
        PlayerSanity sanity = other.GetComponent<PlayerSanity>();

        switch (pickupType)
        {
            case PickupType.Health:
                if (hp != null)
                    hp.currentHealth =
                        Mathf.Min(hp.currentHealth + (int)amount, hp.MaxHealth);

                FindFirstObjectByType<UIManager>()
                    .UpdateHealth(hp.currentHealth, hp.MaxHealth);

                ui.GlowHealth();
                break;

            case PickupType.Stamina:
                if (movement != null)
                    movement.stamina =
                        Mathf.Min(movement.stamina + amount, movement.maxStamina);

                FindFirstObjectByType<UIManager>()
                    .UpdateStamina(movement.stamina, movement.maxStamina);

                ui.GlowStamina();
                break;

            case PickupType.Sanity:
                if (sanity != null)
                    sanity.RestoreSanity(amount);

                ui.GlowSanity();
                break;

            case PickupType.Ammo:
                // later
                break;
        }

        AudioClip soundToPlay = null;

        switch (pickupType)
        {
            case PickupType.Health:
                soundToPlay = medkitSound;
                break;

            case PickupType.Stamina:
                soundToPlay = drinkSound;
                break;

            case PickupType.Sanity:
                soundToPlay = pillSound;
                break;

            case PickupType.Ammo:
                soundToPlay = ammoSound;
                break;
        }

        if (soundToPlay != null)
        {
            AudioSource.PlayClipAtPoint(
                soundToPlay,
                Camera.main.transform.position,
                0.7f
            );
        }

        Destroy(gameObject);
    }
}