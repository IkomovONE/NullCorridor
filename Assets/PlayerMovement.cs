using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public Transform flashlight;
    public SpriteRenderer sr;

    public Transform gunPivot;
    public SpriteRenderer gunSR;

    float idleTimer;
    bool animationLocked = false;

    private Rigidbody2D rb;
    private Vector2 movement;

    bool isInWater = false;
    public RippleLoop ripple;

    public AudioSource waterAudio;
    public AudioClip waterLoop;

    [Header("Ground Detection")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask islandLayer;
    [SerializeField] private LayerMask waterLayer;

    private bool staminaLocked = false;
    private float staminaLockTimer = 0f;

    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 5.5f;
    private float currentSpeed;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float stamina = 100f;
    public float staminaDrain = 25f;
    public float staminaRegen = 18f;

    private UIManager ui;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        gunSR = gunPivot.GetComponentInChildren<SpriteRenderer>();
        ui = FindFirstObjectByType<UIManager>();

        currentSpeed = walkSpeed;
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        Vector2 move = movement;

        // 🔥 CORE LOGIC: what are we standing on?
        bool onIsland = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            islandLayer
        );

        bool onWater = Physics2D.OverlapCircle(
            groundCheck.position,
            checkRadius,
            waterLayer
        );

        isInWater = onWater && !onIsland;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aim = mousePos - transform.position;

        bool sprinting =
            Input.GetKey(KeyCode.LeftShift) &&
            move != Vector2.zero &&
            stamina > 0f;

        // STAMINA
        if (staminaLocked)
        {
            currentSpeed = sprinting ? runSpeed : walkSpeed;
            stamina = maxStamina;

            staminaLockTimer -= Time.deltaTime;
            if (staminaLockTimer <= 0f)
                staminaLocked = false;
        }
        else
        {
            if (sprinting)
            {
                currentSpeed = runSpeed;
                stamina -= staminaDrain * Time.deltaTime;
            }
            else
            {
                currentSpeed = walkSpeed;
                stamina += staminaRegen * Time.deltaTime;
            }
        }

        Debug.Log("Water: " + onWater + " | Island: " + onIsland);

        // 🌊 WATER BEHAVIOR
        if (isInWater)
        {
            currentSpeed *= 0.6f;

            if (movement.magnitude > 0.1f)
            {
                ripple.isActive = true;

                if (!waterAudio.isPlaying)
                {
                    waterAudio.clip = waterLoop;
                    waterAudio.loop = true;
                    waterAudio.Play();
                }

                waterAudio.pitch = sprinting ? 1.4f : 1f;
            }
            else
            {
                ripple.isActive = false;

                if (waterAudio.isPlaying)
                    waterAudio.Stop();
            }
        }
        else
        {
            ripple.isActive = false;

            if (waterAudio.isPlaying)
                waterAudio.Stop();
        }

        stamina = Mathf.Clamp(stamina, 0f, maxStamina);

        if (ui != null)
            ui.UpdateStamina(stamina, maxStamina);

        // 🎯 AIM
        if (aim.x < -0.5f && aim.y > 0.5f)
        {
            sr.flipX = true;
            gunSR.flipY = true;
            flashlight.localRotation = Quaternion.Euler(0, 0, -220);
            flashlight.localPosition = new Vector3(0.40f, 1, 0);
            gunPivot.localRotation = Quaternion.Euler(0, 0, -220);
            gunPivot.localPosition = new Vector3(0.40f, 0.80f, 0);
        }
        else if (aim.x < -0.5f && aim.y < -0.5f)
        {
            sr.flipX = true;
            gunSR.flipY = true;
            flashlight.localRotation = Quaternion.Euler(0, 0, -150);
            flashlight.localPosition = new Vector3(0, 1, 0);
            gunPivot.localRotation = Quaternion.Euler(0, 0, 220);
            gunPivot.localPosition = new Vector3(-0.50f, 1, 0);
        }
        else if (aim.x > 0.5f && aim.y > 0.5f)
        {
            sr.flipX = false;
            gunSR.flipY = false;
            flashlight.localRotation = Quaternion.Euler(0, 0, 60);
            flashlight.localPosition = new Vector3(0.40f, 0, 0);
            gunPivot.localRotation = Quaternion.Euler(0, 0, 60);
            gunPivot.localPosition = new Vector3(0.40f, 0.20f, 0);
        }
        else if (aim.x > 0.5f && aim.y < -0.5f)
        {
            sr.flipX = false;
            gunSR.flipY = false;
            flashlight.localRotation = Quaternion.Euler(0, 0, -20);
            flashlight.localPosition = new Vector3(-0.4f, 0.20f, 0);
            gunPivot.localRotation = Quaternion.Euler(0, 0, -20);
            gunPivot.localPosition = new Vector3(0, 0.20f, 0);
        }
        else if (aim.x < -0.5f)
        {
            sr.flipX = true;
            gunSR.flipY = true;
            gunPivot.localRotation = Quaternion.Euler(0, 1, 180);
            gunPivot.localPosition = new Vector3(0, 1, 0);
            flashlight.localRotation = Quaternion.Euler(0, 1, 180);
            flashlight.localPosition = new Vector3(0, 1, 0);
        }
        else if (aim.x > 0.5f)
        {
            sr.flipX = false;
            gunSR.flipY = false;
            gunPivot.localRotation = Quaternion.Euler(0, 0, 0);
            gunPivot.localPosition = new Vector3(0, 0.1f, 0);
            flashlight.localRotation = Quaternion.Euler(0, 0, 0);
            flashlight.localPosition = new Vector3(0, 0, 0);
        }
        else if (aim.y > 0.5f)
        {
            sr.flipX = false;
            gunSR.flipY = true;
            flashlight.localRotation = Quaternion.Euler(0, 0, 90);
            flashlight.localPosition = new Vector3(0.40f, 1, 0);
            gunPivot.localRotation = Quaternion.Euler(0, 0, 80);
            gunPivot.localPosition = new Vector3(0.70f, 0.20f, 0);
        }
        else if (aim.y < -0.5f)
        {
            sr.flipX = false;
            gunSR.flipY = false;
            flashlight.localRotation = Quaternion.Euler(0, 0, -90);
            flashlight.localPosition = new Vector3(-0.60f, 0.70f, 0);
            gunPivot.localRotation = Quaternion.Euler(0, 0, -90);
            gunPivot.localPosition = new Vector3(-0.30f, 0.65f, 0);
        }

        // 🎬 ANIMATION
        if (!animationLocked)
        {
            if (move != Vector2.zero)
            {
                animator.SetInteger("Animate", 2);
                idleTimer = 0f;
            }
            else
            {
                idleTimer += Time.deltaTime;

                if (idleTimer > 5f)
                    animator.SetInteger("Animate", 3);
                else
                    animator.SetInteger("Animate", 0);
            }
        }
    }

    public void LockStamina(float seconds)
    {
        staminaLocked = true;
        staminaLockTimer = seconds;
        stamina = maxStamina;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * currentSpeed;
    }
}