using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EntryPrologue : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public TutorialTrigger firstTutorialTrigger;

    public GameObject gun;
    public GameObject flashlight;

    public BoxCollider2D blockingCollider;
    public AudioClip DoorSound;

    public Image LevelEntryUI;

    public GameObject LevelEntryUIObj;

    private AudioSource audioSource;

    public float defaultCameraSize = 5f;
    public float zoomedCameraSize = 1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(EntryPrologueSequence());
    }

    IEnumerator EntryPrologueSequence()
    {
        Camera cam = Camera.main;

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        SpriteRenderer psr = player.GetComponent<SpriteRenderer>();

        // Disable movement during intro
        if (movement != null)
            movement.enabled = false;

        // Start zoomed in
        cam.orthographicSize = zoomedCameraSize;
        LevelEntryUIObj.SetActive(true);
        StartCoroutine(FadeFromBlack());
        // Player starts invisible
        Color c = psr.color;
        c.a = 0f;
        psr.color = c;
        gun.SetActive(false);
        flashlight.SetActive(false);

        yield return new WaitForSeconds(4f);
        // Open door + sound
        audioSource.PlayOneShot(DoorSound);
        animator.SetTrigger("Open");
        

        yield return new WaitForSeconds(0.35f);
        gun.SetActive(true);
        flashlight.SetActive(true);

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
        
    }

    IEnumerator FadeFromBlack()

    {

        float duration = 5f;

        float t = 0f;

        Color c = LevelEntryUI.color;

        while (t < duration)

        {

            t += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, t / duration);

            c.a = alpha;

            LevelEntryUI.color = c;

            yield return null;

        }

        

        c.a = 0f;

        LevelEntryUI.color = c;

        LevelEntryUIObj.SetActive(false);

        if (firstTutorialTrigger != null)

        {

            firstTutorialTrigger.TriggerManually();

        }

            }
}