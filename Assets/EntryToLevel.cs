using UnityEngine;
using System.Collections;


//This class is used to handle the entry door for the level, which plays an animation, moves the player, and displays a level entry UI when the game starts.
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

        
        if (movement != null)
            movement.enabled = false;

        
        cam.orthographicSize = zoomedCameraSize;
        LevelEntryUI.SetActive(true);
        
        Color c = psr.color;
        c.a = 0f;
        psr.color = c;

        
        audioSource.PlayOneShot(DoorSound);
        animator.SetTrigger("Open");

        yield return new WaitForSeconds(0.35f);

        
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
        LevelEntryUI.SetActive(false);
    }
}