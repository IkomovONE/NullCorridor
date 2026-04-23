using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    public LayerMask wallLayer;
    public Transform player;

    private Animator animator;
    private SpriteRenderer sr;

    [Header("Movement")]
    public float patrolSpeed = 1.5f;
    public float chaseSpeed = 2.5f;
    public float chaseRadius = 5f;
    public float attackRadius = 1.3f;
    public float patrolRange = 3f;

    public AudioClip chaseSound;
    public AudioClip attackSound;

    private AudioSource audioSource;
    private bool wasChasing = false;

    private Vector3 patrolCenter;
    private bool movingRight = true;

    private float idleTimer = 0f;
    private float moveTimer = 3f;

    [Header("Attack")]

    public int damage = 1;

    private float attackRecoverTimer = 0;

    public float attackCooldown = 0.8f;

    private float nextAttackTime = 0.5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        patrolCenter = transform.position;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 dir = player.position - transform.position;
        float dist = dir.magnitude;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir.normalized,
            dist,
            wallLayer
        );

        return hit.collider == null;
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        // ATTACK
        if (dist <= attackRadius)
        {
            animator.SetBool("isMoving", false);
            Attack();
            return;
        }
        else
        {
            animator.SetBool("isAttacking", false);
        }

        // CHASE
        if (dist <= chaseRadius && CanSeePlayer())

        {

            if (!wasChasing)

            {

                audioSource.PlayOneShot(chaseSound);

                wasChasing = true;

            }

            Chase();

            return;

        }

        else

        {

            wasChasing = false;

        }

        // PATROL
        Patrol();
    }

    void Chase()
    {
        animator.SetBool("isMoving", true);
        animator.speed = 1.5f;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            chaseSpeed * Time.deltaTime
        );

        if (player.position.x > transform.position.x)
            sr.flipX = true;
        else
            sr.flipX = false;
    }

    void Attack()
    {
        // If recovering after previous hit
        if (attackRecoverTimer > 0)
        {
            attackRecoverTimer -= Time.deltaTime;

            animator.SetBool("isAttacking", true);
            animator.SetBool("isMoving", false);
            animator.speed = 1f;

            return;
        }

        // Ready to strike
        animator.SetBool("isAttacking", true);
        animator.SetBool("isMoving", false);
        audioSource.PlayOneShot(attackSound);
        animator.speed = 1f;

        if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;

            if (player != null)
            {
                PlayerHealth hp = player.GetComponent<PlayerHealth>();

                if (hp != null)
                {
                    hp.TakeDamage(damage);
                    hp.Knockback(transform.position, 0.3f);

                    animator.SetBool("isAttacking", false);

                    attackRecoverTimer = Random.Range(0.1f, 0.3f);
                }
            }
        }
    }

    void Patrol()
    {
        // idle phase
        if (idleTimer > 0f)
        {
            idleTimer -= Time.deltaTime;
            animator.SetBool("isMoving", false);
            return;
        }

        animator.SetBool("isMoving", true);
        animator.speed = 0.15f;

        float dir = movingRight ? 1f : -1f;

        transform.Translate(Vector2.right * dir * patrolSpeed * Time.deltaTime);

        if (movingRight)
            sr.flipX = true;
        else
            sr.flipX = false;

        moveTimer -= Time.deltaTime;

        if (moveTimer <= 0f)
        {
            idleTimer = 2f;     // pause for 2 sec
            moveTimer = 3f;     // move for 3 sec
            movingRight = !movingRight;
        }

        // keep patrol near current area (no teleport)
        float distFromCenter = transform.position.x - patrolCenter.x;

        if (distFromCenter > patrolRange)
            movingRight = false;

        if (distFromCenter < -patrolRange)
            movingRight = true;
    }
}