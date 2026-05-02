using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class ExitDoor : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public GameObject levelCompleteUI;

    public string levelID;

    public string nextSceneName;

    public int unlockLevelNumber = 1;

    public TMP_Text progressText;

    public BoxCollider2D blockingCollider;
    public AudioClip DoorSound;

    private bool playerNear = false;
    private bool activated = false;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (activated) return;

        if (playerNear && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(FinishSequence());
        }
    }

    IEnumerator FinishSequence()
    {
        activated = true;

        Camera cam = Camera.main;

        PlayerMovement movement = player.GetComponent<PlayerMovement>();
        SpriteRenderer psr = player.GetComponent<SpriteRenderer>();

        // Disable movement immediately
        if (movement != null)
            movement.enabled = false;

        // Remove door collision
        blockingCollider.enabled = false;

        // Play door sound + animation together
        audioSource.PlayOneShot(DoorSound);
        animator.SetTrigger("Open");

        // Camera zoom while door opens
        float startSize = cam.orthographicSize;
        float targetSize = startSize - 2f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 1.2f;

            cam.orthographicSize =
                Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        // Walk player into door
        while (Vector2.Distance(player.position, transform.position) > 0.3f)
        {
            player.position = Vector2.MoveTowards(
                player.position,
                transform.position,
                2f * Time.deltaTime
            );

            yield return null;
        }

        // Fade player out
        float fade = 1f;

        while (fade > 0f)
        {
            fade -= Time.deltaTime * 2f;

            Color c = psr.color;
            c.a = fade;
            psr.color = c;

            yield return null;
        }

        player.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.3f);

        

        UIManager ui = FindFirstObjectByType<UIManager>();

        int percent = 50;
        int found = 0;
        int total = 0;

        if (ui != null)

            percent = ui.GetCompletionPercent();
            found = ui.levelPagesFound;
            total = ui.totalLevelPages;

        progressText.text = "Score: " + percent + "% (" + found + " of " + total + " diaries found)";

        Time.timeScale = 0f;
        levelCompleteUI.SetActive(true);

        SaveSystem.SaveLevelStats(levelID, percent, found, total);
        SaveSystem.SaveProgress(levelID, unlockLevelNumber);

        yield return new WaitForSecondsRealtime(3f);


        Time.timeScale = 1f;
        SceneManager.LoadScene(nextSceneName);

        
        

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered door zone");
            playerNear = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}