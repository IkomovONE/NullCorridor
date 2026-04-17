using UnityEngine;
using UnityEngine.SceneManagement;

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