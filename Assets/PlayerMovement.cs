using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
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
        }
        else if (movement.x > 0)
        {
            sr.flipX = false;
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