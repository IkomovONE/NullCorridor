using UnityEngine;
using TMPro;
using System.Collections;


//This class defines the behaviour of the story trigger objects and states each story message text.
public class StoryTrigger : MonoBehaviour
{
    public int StoryID = 1;
    public GameObject StoryPanel;
    public TMP_Text StoryText;
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

        
    }

    void ShowMessage()
    {
        string msg = "";
        switch (StoryID) //The story messages are written with help of AI (ChatGPT)
        {
            case 1:
                msg = "Where am I...\nThis doesn’t look like any place I know.";
                break;

            case 2:
                msg = "How did I even fall into this place..?\nA moment ago we were securing a breach point...\nCommand said it was just a spatial anomaly, how the hell did I end up here?";
                break;

            case 3:
                msg = "And why is this hallway so damn long...\nTurn after turn, it just keeps going...\nI can hear something breathing in the distance...";
                break;

            case 4:
                msg = "Wait, who's that?\nHello? Can you hear me?";
                break;

            case 5:
                msg = "...";
                break;

            case 6:
                msg = "Damn...\nThat wasn't a human...\nIt was too tall, too still...\nIt only moved when I blinked...";
                break;

            case 7:
                msg = "And who left all this stuff here?";
                break;

            case 8:
                msg = "What's that, someone's diary?\nMaybe it can tell me something about this place...";
                break;

            case 9:
                msg = "A door, finally!";
                break;

            case 10:
                msg = "What?!?\nAnother hallway?!?\nThis place is a maze...\nI have to find a way out of here...";
                break;

            case 11:
                msg = "Who wrote this?!?\nSomebody was either going crazy here, or they were trying to warn someone else...\nEither way, this place is dangerous...\nI have to get out of here...";
                break;

            case 12:
                msg = "Is this the right way...? I hope so...";
                break;

            case 13:
                msg = "????\n A yellow protective suit?\n A gas mask?\n Who would need this stuff in a place like this?";
                break;

            case 14:
                msg = "Is this door gonna lead me out of here...?\n Looks the same as all the others, but maybe it’s different on the other side...";
                break;

            case 15:
                msg = "Wow, what?!?!\n Pools??!?!\n How is that even possible in a place like this??";
                break;

            case 16:
                msg = "This place is so calm yet so terrifying at the same time...";
                break;

            case 17:
                msg = "That’s not good...\n I can hear something behind the wall walking in the water...\n It’s getting louder...";
                break;

            default:
                msg = "Tutorial";
                break;
        }

        StartCoroutine(ShowPopup(msg));
    }

    IEnumerator ShowPopup(string msg)
    {
        StoryPanel.SetActive(true);
        StoryText.text = msg;
        Time.timeScale = 0f;

        savedCamPos = Camera.main.transform.position;
        savedCamRot = Camera.main.transform.rotation;
        freezeCamera = true;

       yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

        StoryPanel.SetActive(false);
        freezeCamera = false;
        Time.timeScale = 1f;
    }

    IEnumerator ShowPopupDelayed(string msg, float delay)
    {

        yield return new WaitForSeconds(delay);
        yield return StartCoroutine(ShowPopup(msg));

    }
}