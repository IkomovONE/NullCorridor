using UnityEngine;
using UnityEngine.SceneManagement;


//This class is used to handle the exit trigger for the level, which displays a win message and stops time when the player reaches it.
public class ExitTrigger : MonoBehaviour
{
    public GameObject winText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            winText.SetActive(true);
            Debug.Log("Level Completed!");
            Time.timeScale = 0f;
        }
    }
}