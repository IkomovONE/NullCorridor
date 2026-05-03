using UnityEngine;


//This class is used to handle the pickups in the game, which can restore health, stamina, sanity, ammo, or be a diary entry.
public class PickupItem : MonoBehaviour
{
    public enum PickupType
    {
        Health,
        Stamina,
        Sanity,
        Ammo,
        Diary
    }

    public AudioClip medkitSound;
    public AudioClip drinkSound;
    public AudioClip pillSound;
    public AudioClip ammoSound;
    public AudioClip DiarySound;
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
        PlayerGun gun = other.GetComponent<PlayerGun>();

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

                if (movement != null) movement.LockStamina(30f);

                ui.GlowStamina();
                break;

            case PickupType.Sanity:
                if (sanity != null)
                    sanity.RestoreSanity(amount);

                ui.GlowSanity();
                break;

            case PickupType.Ammo:

                gun.AddAmmo(2);
                
                break;

            case PickupType.Diary:

                DiaryPickup dp = GetComponent<DiaryPickup>();

                if (dp != null)

                {

                    UIManager ui = FindFirstObjectByType<UIManager>();

                    if (ui != null)
                        ui.CollectDiary(dp.diaryID);

                }
                
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

            case PickupType.Diary:
                soundToPlay = DiarySound;
                break;
        }

        if (soundToPlay != null)
        {
            AudioSource.PlayClipAtPoint(soundToPlay, Camera.main.transform.position, 0.7f);
        }

        Destroy(gameObject);
    }
}