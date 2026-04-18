using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public Transform flashlight;
    float idleTimer;
    public float speed = 5f;
    bool animationLocked = false;
    public SpriteRenderer sr;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        sr = GetComponent<SpriteRenderer>();

        if (movement.x < 0)
        {
            sr.flipX = true;
            flashlight.localRotation = Quaternion.Euler(0, 1, 180);
            flashlight.localPosition = new Vector3(0, 1, 0);

            if (movement.y > 0)
            {
                flashlight.localRotation = Quaternion.Euler(0, 0, -220);
                flashlight.localPosition = new Vector3(0.40f, 1, 0);

            }

            else if (movement.y < 0)
            {
                flashlight.localRotation = Quaternion.Euler(0, 0, -150);
                flashlight.localPosition = new Vector3(0, 1, 0);

            }
            
            
           
        }
        else if (movement.x > 0)
        {
            sr.flipX = false;
            flashlight.localRotation = Quaternion.Euler(0, 0, 0);
            flashlight.localPosition = new Vector3(0, 0, 0);

            if (movement.y > 0)
            {
                sr.flipX = false;
                flashlight.localRotation = Quaternion.Euler(0, 0, 60);
                flashlight.localPosition = new Vector3(0.40f, 0, 0);

            }

            else if (movement.y < 0)
            {
                sr.flipX = false;
                flashlight.localRotation = Quaternion.Euler(0, 0, -20);
                flashlight.localPosition = new Vector3(-0.4f, 0.20f, 0);

            }
            
        }

        else if (movement.y > 0)
        {
            sr.flipX = false;
            flashlight.localRotation = Quaternion.Euler(0, 0, 90);
            flashlight.localPosition = new Vector3(0.40f, 1, 0);
            
        }

        else if (movement.y < 0)
        {
            sr.flipX = false;
            flashlight.localRotation = Quaternion.Euler(0, 0, -90);
            flashlight.localPosition = new Vector3(-0.60f, 0.70f, 0);
            
        }


        if (!animationLocked)
        {

            if (move != Vector2.zero)
            {
                animator.SetInteger("Animate", 2); // Walk
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
        rb.linearVelocity = movement.normalized * speed;
    }
}