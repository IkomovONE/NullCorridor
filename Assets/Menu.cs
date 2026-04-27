using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    public Slider volumeSlider;

    public Slider brightnessSlider;

    public Image brightnessOverlay;
    public GameObject mainPanel;
    public GameObject levelsPanel;
    public GameObject settingsPanel;

    public GameObject QuitPanel;

    void Start()

    {

        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);

        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.2f);

        volumeSlider.value = savedVolume;

        brightnessSlider.value = savedBrightness;

        SetVolume(savedVolume);

        SetBrightness(savedBrightness);

    }

    public void SetVolume(float value)

    {

        AudioListener.volume = value;

        PlayerPrefs.SetFloat("Volume", value);

    }

    public void SetBrightness(float value)

    {

        Color c = brightnessOverlay.color;

        c.a = Mathf.Lerp(0.45f, 0f, value);

        brightnessOverlay.color = c;

        PlayerPrefs.SetFloat("Brightness", value);

    }

    public void PlayNewGame()
    {
        SceneManager.LoadScene("Level1- Tutorial Lobby");
    }

    public void Continue()
    {
        SceneManager.LoadScene("Level1");
    }

    public void LoadLevel(string sceneName)
    {

        SceneManager.LoadScene(sceneName);
    }

    public void Levels()
    {
        mainPanel.SetActive(false);
        settingsPanel.SetActive(false);
        QuitPanel.SetActive(false);
        levelsPanel.SetActive(true);
        
    }

    public void Settings()
    {
        mainPanel.SetActive(false);
        levelsPanel.SetActive(false);
        QuitPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToMain()
    {
        levelsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        QuitPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    public void Quit()
    {
        mainPanel.SetActive(false);
        levelsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        QuitPanel.SetActive(true);
        Application.Quit();
    }
}