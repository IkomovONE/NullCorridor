using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [Header("Gun")]
    public int ammo = 50;
    public int maxAmmo = 100;

    public Transform gun;
    public float shootRange = 5f;
    public int damage = 1;

    public LayerMask shootMask;

    private Vector2 movement;

    private PlayerMovement move;
    public GameObject tracerPrefab;

    

    [Header("Audio")]

    public AudioClip shootSound;

    public AudioClip emptyClickSound;
   

    private UIManager ui;
    public AudioSource GunAudioSource;
    void Start()
    {
        ui = FindFirstObjectByType<UIManager>();
        

   
        if (ui != null)
            ui.UpdateAmmo(ammo, maxAmmo);
    }

    void Update()

    {

        if (Time.timeScale == 0f) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
       

    }

    void RefreshUI()

    {

        if (ui != null)

            ui.UpdateAmmo(ammo, maxAmmo);

    }

    
    public void AddAmmo(int amount)
    {
        

        ammo = Mathf.Clamp(ammo + amount, 0, maxAmmo);

        RefreshUI();
        
    }

    // Optional if enemy events damage sanity later
    public void Shoot()
    {

        if (ammo <= 0)

        {

            if (emptyClickSound != null)

                GunAudioSource.PlayOneShot(emptyClickSound);

            return;

        }

        ammo--;
        

        Vector3 shotOrigin = gun.position + new Vector3(0f, 0.08f, 0f);;

        

        

        RaycastHit2D hit = Physics2D.Raycast(
            shotOrigin,
            gun.right,
            shootRange,
            shootMask
        );
        Instantiate(tracerPrefab, shotOrigin, gun.rotation);

        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.name);
            EnemyMove enemy = hit.collider.GetComponent<EnemyMove>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
        else Debug.Log("Missed");

        RefreshUI();

        if (shootSound != null)

            GunAudioSource.PlayOneShot(shootSound);

    }
}