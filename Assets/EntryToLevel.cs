using UnityEngine;
using System.Collections;

public class EntryDoor : MonoBehaviour
{
    public Animator animator;
    public Transform player;

    public BoxCollider2D blockingCollider;
    public AudioClip DoorSound;

    public GameObject LevelEntryUI;

    private AudioSource audioSource;

    public float defaultCameraSize = 5f;
    public float zoomedCameraSize = 3f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(EntrySequence());
    }

    IEnumerator EntrySequence()
    {
        Camera cam = Camera.main;

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        SpriteRenderer psr = player.GetComponent<SpriteRenderer>();

        // Disable movement during intro
        if (movement != null)
            movement.enabled = false;

        // Start zoomed in
        cam.orthographicSize = zoomedCameraSize;
        LevelEntryUI.SetActive(true);
        // Player starts invisible
        Color c = psr.color;
        c.a = 0f;
        psr.color = c;

        // Open door + sound
        audioSource.PlayOneShot(DoorSound);
        animator.SetTrigger("Open");

        yield return new WaitForSeconds(0.35f);

        // Allow player through
        blockingCollider.enabled = false;

        // Move player outward from door while fading in
        Vector3 targetPos = player.position + new Vector3(-1f, -0.2f, 0f);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;

            // Walk outward
            player.position = Vector3.Lerp(
                transform.position,
                targetPos,
                t
            );

            // Fade in
            Color fc = psr.color;
            fc.a = t;
            psr.color = fc;

            // Zoom camera back to normal
            cam.orthographicSize =
                Mathf.Lerp(zoomedCameraSize, defaultCameraSize, t);

            yield return null;
        }

        // Re-enable movement
        if (movement != null)
            movement.enabled = true;

        yield return new WaitForSeconds(2f);
        LevelEntryUI.SetActive(false);
    }
}