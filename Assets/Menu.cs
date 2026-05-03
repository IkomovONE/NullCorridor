using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//This class defines the main menu of the game, including level selection, settings, and quitting the game.
public class Menu : MonoBehaviour
{

    public Slider volumeSlider;
    public Slider brightnessSlider;
    public Image brightnessOverlay;
    public GameObject mainPanel;
    public GameObject levelsPanel;
    public GameObject settingsPanel;
    public Button tutorialButton;
    public Button lobbyButton;
    public Button poolsButton;
    public TMP_Text tutorialProgressText;
    public TMP_Text lobbyProgressText;
    public TMP_Text poolsProgressText;
    public GameObject QuitPanel;

    void Start()
    {

        int unlocked = SaveSystem.GetUnlockedLevel();
        SetButtonState(tutorialButton, true);
        SetButtonState(lobbyButton, unlocked >= 2);
        SetButtonState(poolsButton, unlocked >= 3);
        LoadLevelText("Tutorial", tutorialProgressText);
        LoadLevelText("Lobby", lobbyProgressText);
        LoadLevelText("Pools", poolsProgressText);
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

    void SetButtonState(Button btn, bool unlocked)
    {
        btn.interactable = unlocked;
        Image img = btn.GetComponent<Image>();
        Color c = img.color;
        c.a = unlocked ? 1f : 0.35f;
        img.color = c;
    }


    public void PlayNewGame()
    {
        SaveSystem.ResetSave();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level1- Tutorial Lobby");
    }


    public void Continue()
    {
        SceneManager.LoadScene(SaveSystem.GetLastScene());
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

    void LoadLevelText(string levelID, TMP_Text txt)
    {

        int percent = SaveSystem.GetStat(levelID + "_Percent");
        int found = SaveSystem.GetStat(levelID + "_Found");
        int total = SaveSystem.GetStat(levelID + "_Total");

        if (total > 0)

        {
            txt.text = percent + "% completed (" + found + "/" + total + " diaries)";
        }

        else

        {
            txt.text = "Not completed";
        }

    }
}