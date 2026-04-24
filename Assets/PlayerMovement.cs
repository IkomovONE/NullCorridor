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

    public void LockAnimation(float time)
    {
        animationLocked = true;
        Invoke("UnlockAnimation", time);
    }

    void UnlockAnimation()
    {
        animationLocked = false;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        Vector2 move = movement;

        // STAMINA + SPRINT
        bool sprinting =
            Input.GetKey(KeyCode.LeftShift) &&
            move != Vector2.zero &&
            stamina > 0f;

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

        stamina = Mathf.Clamp(stamina, 0f, maxStamina);

        if (ui != null)
            ui.UpdateStamina(stamina, maxStamina);

        // SPRITE + FLASHLIGHT
        if (movement.x < 0)
        {
            sr.flipX = true;
            //gunPivot.localRotation = Quaternion.Euler(0,0,45);
            gunSR.flipY = true;
            gunPivot.localRotation = Quaternion.Euler(0, 1, 180);
            gunPivot.localPosition = new Vector3(0, 1, 0);

            flashlight.localRotation = Quaternion.Euler(0, 1, 180);
            flashlight.localPosition = new Vector3(0, 1, 0);

            if (movement.y > 0)
            {
                flashlight.localRotation = Quaternion.Euler(0, 0, -220);
                flashlight.localPosition = new Vector3(0.40f, 1, 0);

                gunPivot.localRotation = Quaternion.Euler(0, 0, -220);
                gunPivot.localPosition = new Vector3(0.40f, 0.80f, 0);
            }
            else if (movement.y < 0)
            {

                
                flashlight.localRotation = Quaternion.Euler(0, 0, -150);
                flashlight.localPosition = new Vector3(0, 1, 0);

                gunPivot.localRotation = Quaternion.Euler(0, 0, 220);
                gunPivot.localPosition = new Vector3(-0.50f, 1, 0);
            }
        }
        else if (movement.x > 0)
        {
            sr.flipX = false;
            gunSR.flipY = false;
            gunPivot.localRotation = Quaternion.Euler(0, 0, 0);
            gunPivot.localPosition = new Vector3(0, 0.1f, 0);

            flashlight.localRotation = Quaternion.Euler(0, 0, 0);
            flashlight.localPosition = new Vector3(0, 0, 0);

            if (movement.y > 0)
            {
                flashlight.localRotation = Quaternion.Euler(0, 0, 60);
                flashlight.localPosition = new Vector3(0.40f, 0, 0);

                gunPivot.localRotation = Quaternion.Euler(0, 0, 60);
                gunPivot.localPosition = new Vector3(0.40f, 0.20f, 0);
            }
            else if (movement.y < 0)
            {
                flashlight.localRotation = Quaternion.Euler(0, 0, -20);
                flashlight.localPosition = new Vector3(-0.4f, 0.20f, 0);

                gunPivot.localRotation = Quaternion.Euler(0, 0, -20);
                gunPivot.localPosition = new Vector3(0, 0.20f, 0);
            }
        }
        else if (movement.y > 0)
        {
            sr.flipX = false;
            flashlight.localRotation = Quaternion.Euler(0, 0, 90);
            flashlight.localPosition = new Vector3(0.40f, 1, 0);
            gunSR.flipY = true;
            gunPivot.localRotation = Quaternion.Euler(0, 0, 80);
            gunPivot.localPosition = new Vector3(0.70f, 0.20f, 0);
        }
        else if (movement.y < 0)
        {
            sr.flipX = false;
            flashlight.localRotation = Quaternion.Euler(0, 0, -90);
            flashlight.localPosition = new Vector3(-0.60f, 0.70f, 0);
            gunSR.flipY = false;
            gunPivot.localRotation = Quaternion.Euler(0, 0, -90);
            gunPivot.localPosition = new Vector3(-0.30f, 0.65f, 0);
        }

        // ANIMATION
        if (!animationLocked)
        {
            if (move != Vector2.zero)
            {
                animator.SetInteger("Animate", 2); // Walk/Run
                idleTimer = 0f;
            }
            else
            {
                idleTimer += Time.deltaTime;

                if (idleTimer > 5f)
                    animator.SetInteger("Animate", 3); // Bored
                else
                    animator.SetInteger("Animate", 0); // Idle
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement.normalized * currentSpeed;
    }
}