using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//This class is used to handle the entry door for the prologue, which plays an animation, moves the player, and displays a level entry UI when the game starts.
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

       
        if (movement != null)
            movement.enabled = false;

        
        cam.orthographicSize = zoomedCameraSize;
        LevelEntryUIObj.SetActive(true);
        StartCoroutine(FadeFromBlack());
        
        Color c = psr.color;
        c.a = 0f;
        psr.color = c;
        gun.SetActive(false);
        flashlight.SetActive(false);

        yield return new WaitForSeconds(4f);
        
        audioSource.PlayOneShot(DoorSound);
        animator.SetTrigger("Open");
        

        yield return new WaitForSeconds(0.35f);
        gun.SetActive(true);
        flashlight.SetActive(true);

        
        blockingCollider.enabled = false;

        
        Vector3 targetPos = player.position + new Vector3(-1f, -0.2f, 0f);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime;

            
            player.position = Vector3.Lerp(
                transform.position,
                targetPos,
                t
            );

            
            Color fc = psr.color;
            fc.a = t;
            psr.color = fc;

            
            cam.orthographicSize =
                Mathf.Lerp(zoomedCameraSize, defaultCameraSize, t);

            yield return null;
        }

        
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