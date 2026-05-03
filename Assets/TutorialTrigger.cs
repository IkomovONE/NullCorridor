using UnityEngine;
using TMPro;
using System.Collections;

//This class defines the behaviour of the tutorial trigger objects and states each tutorial message text.
public class TutorialTrigger : MonoBehaviour
{
    public int tutorialID = 1;
    public GameObject tutorialPanel;
    public TMP_Text tutorialText;
    private Vector3 savedCamPos;
    private Quaternion savedCamRot;
    private bool freezeCamera = false;
    public float showTime = 4f;
    public bool triggerOnce = true;
    private bool used = false;



    void Update()

    {

        if (freezeCamera && Camera.main != null)
        {
            Camera.main.transform.position = savedCamPos;
            Camera.main.transform.rotation = savedCamRot;
        }

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (used && triggerOnce) return;

        used = true;
        ShowMessage();

        
        if (tutorialID == 4)
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();

            if (hp != null)
                hp.TakeDamage(50);
        }

        if (tutorialID == 5)
        {
            PlayerSanity san = other.GetComponent<PlayerSanity>();

            if (san != null)
                san.LoseSanity(60);
        }
    }


    public void TriggerManually()

    {
        if (used && triggerOnce) return;

        used = true;
        ShowMessage();
    }


    void ShowMessage()
    {
        string msg = "";
        switch (tutorialID)  //Tutorial messages are written with help of AI (ChatGPT)
        {
            case 1:
                msg = "WASD to walk\n\nMove mouse to look around.\n\nESC to open pause menu.";
                break;

            case 2:
                msg = "Hold LEFT SHIFT to sprint\n\nStamina will drain while sprinting.";
                break;

            case 3:
                msg = "Pick up ENERGY DRINK by walking over it\n\nRestores stamina and boosts it.";
                break;

            case 4:
                msg = "Pick up MEDKIT\n\nRestores lost health.";
                break;

            case 5:
                msg = "Pick up PILLS\n\nRestore sanity. Harder to see and aim when You're insane.";
                break;

            case 6:
                msg = "Pick up AMMO\n\nKeep your gun loaded.";
                break;

            case 7:
                msg = "LEFT CLICK to shoot\n\nMove mouse to aim and kill the enemy.";
                break;

            case 8:
                msg = "Powerups can spawn randomly across the map.";
                break;

            case 9:
                msg = "Pick up DIARY pages\n\nIncrease score and progress through the story.";
                break;

            case 10:
                msg = "Press E near the door to finish the escape.";
                break;

            default:
                msg = "Tutorial";
                break;
        }

        if (tutorialID == 1)
            StartCoroutine(ShowPopupDelayed(msg, 2f));

        else
            StartCoroutine(ShowPopup(msg));
    }

    IEnumerator ShowPopup(string msg)
    {
        tutorialPanel.SetActive(true);
        tutorialText.text = msg;
        Time.timeScale = 0f;
        savedCamPos = Camera.main.transform.position;
        savedCamRot = Camera.main.transform.rotation;
        freezeCamera = true;
       yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        tutorialPanel.SetActive(false);
        freezeCamera = false;
        Time.timeScale = 1f;
    }

    IEnumerator ShowPopupDelayed(string msg, float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(ShowPopup(msg));
    }
}