using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;


//This class defines the main UI logics of the game, such as player's vitals, diary UI, pause menu, and level completion UI.
public class UIManager : MonoBehaviour
{
    public Image healthFill;
    public Image staminaFill;
    public Image sanityFill;
    public TMP_Text ammoText;
    public int diaryPages = 0;
    public int levelPagesFound = 0;
    public int totalLevelPages = 0;
    public GameObject pauseMenuPanel;
    public GameObject diaryOverlay;
    public TMP_Text diaryText;
    private bool diaryOpen = false;
    private Vector3 pausedCameraPos;
    private Quaternion pausedCameraRot;
    private bool paused = false;
    public Image brightnessOverlay;
    public Slider brightnessSlider;
    public Slider volumeSlider;
    public TMP_Text pauseProgressText;
    Coroutine hpGlow;
    Coroutine stmGlow;
    Coroutine sanGlow;
    private float smoothSpeed = 4f;
    private float targetHealth = 2f;

    void Start()
    {
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", 0.5f);
        SetBrightness(savedBrightness);

        if (brightnessSlider != null)
            brightnessSlider.value = savedBrightness;

        if (diaryOverlay != null)
            diaryOverlay.SetActive(false);

        DiaryPickup[] diaries = FindObjectsByType<DiaryPickup>(FindObjectsSortMode.None);
        totalLevelPages = diaries.Length;
        levelPagesFound = 0;
    }




    void Update()
    {
        healthFill.fillAmount = Mathf.Lerp(healthFill.fillAmount, targetHealth, Time.deltaTime * smoothSpeed);

        if (Input.GetKeyDown(KeyCode.Escape) && !diaryOpen)
        {
            TogglePause();
        }
        if (diaryOpen && Input.GetKeyDown(KeyCode.Space))
        {
            CloseDiary();
        }
        if ((paused || diaryOpen) && Camera.main != null)
        {
            Camera.main.transform.position = pausedCameraPos;
            Camera.main.transform.rotation = pausedCameraRot;
        }
    }

    public void UpdateHealth(float current, float max)
    {
        targetHealth = current / max;
    }


    public void UpdateStamina(float current, float max)
    {
        staminaFill.fillAmount = current / max;
    }


    public void UpdateSanity(float current, float max)
    {
        sanityFill.fillAmount = current / max;
    }


    public void UpdateAmmo(float current, float max)
    {
        ammoText.text = current + "/" + max;
        if (current < 1)
            ammoText.color = Color.red;
        else
            ammoText.color = new Color32(138, 138, 138, 255);
    }


    public void GlowHealth()
    {
        if (hpGlow != null) StopCoroutine(hpGlow);
        hpGlow = StartCoroutine(
            GlowBar(healthFill, new Color(1f, 0.4f, 0.4f))
        );
    }


    public void GlowStamina()
    {
        if (stmGlow != null) StopCoroutine(stmGlow);
        stmGlow = StartCoroutine(
            GlowBar(staminaFill, new Color(0.4f, 0.7f, 1f))
        );
    }


    public void GlowSanity()
    {
        if (sanGlow != null) StopCoroutine(sanGlow);
        sanGlow = StartCoroutine(
            GlowBar(sanityFill, new Color(0.8f, 0.4f, 1f))
        );
    }

    IEnumerator GlowBar(Image bar, Color glowColor)
    {
        Color original = bar.color;
        bar.color = glowColor;
        yield return new WaitForSeconds(0.18f);
        bar.color = original;
    }


    public void TogglePause()
    {
        paused = !paused;
        pauseMenuPanel.SetActive(paused);

        if (paused)
        {
            pausedCameraPos = Camera.main.transform.position;
            pausedCameraRot = Camera.main.transform.rotation;
            Time.timeScale = 0f;

            if (pauseProgressText != null)
            {
                pauseProgressText.text = GetCompletionPercent() + "% completed (" + levelPagesFound + "/" + totalLevelPages + " diaries)";

            }
        }
        else
        {
            Time.timeScale = 1f;
        }
    }


    public void ResumeGame()
    {
        paused = false;

        pauseMenuPanel.SetActive(false);

        Time.timeScale = 1f;
    }


    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
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


    public void CollectDiary(int diaryID)
    {

        diaryPages++;
        levelPagesFound++;
        PlayerPrefs.SetInt("DiaryPages", diaryPages);
        string message = GetDiaryText(diaryID);

        ShowDiary(message);

    }


    public string GetDiaryText(int id)
    {
        switch (id)  //The diary entries are written with help of AI (ChatGPT)
        {
            case 1:
                return "Day 1.\n\nI found this notebook in my pocket.\nNo doors lead outside.\nThe lights hum even when the power dies.";

            case 2:
                return "Day 3.\n\nThe rooms repeat themselves.\nI marked one wall, then found the mark again\nthree corridors away.";

            case 3:
                return "Day 6.\n\nFood appears in vending machines at random.\nI no longer ask where it comes from.\nI only eat before it vanishes.";

            case 4:
                return "Day 11.\n\nSomething smiled at me from the far hall.\nToo tall. Too still.\nIt moved only when I blinked.";

            case 5:
                return "Day 15.\n\nI hear water below the floor.\nThere are no pipes here.\nSometimes it sounds like breathing.";

            case 6:
                return "Day 18.\n\nI found stairs descending into warm fog.\nThe air tastes of chlorine and rust.\nI should have turned back.";

            case 7:
                return "Day 21.\n\nThe pools stretch farther than buildings should.\nWaves form even when nothing moves.";

            case 8:
                return "Day 27.\n\nI saw myself standing across the water.\nHe waved first.";

            case 9:
                return "Day 34.\n\nIf you find this, do not follow voices.\nThey learn your memories first.\nThen they use your name.";

            default:
                return "Day ?.\n\nThe pages are unreadable.";
        }
    }


    public void ShowDiary(string message)

    {

        if (diaryOverlay == null) return;
        diaryOpen = true;
        pausedCameraPos = Camera.main.transform.position;
        pausedCameraRot = Camera.main.transform.rotation;
        paused = false;
        pauseMenuPanel.SetActive(false);
        diaryOverlay.SetActive(true);
        if (diaryText != null)

            diaryText.text = message;

        Time.timeScale = 0f;
    }


    public void CloseDiary()
    {
        diaryOpen = false;
        if (diaryOverlay != null)
            diaryOverlay.SetActive(false);

        Time.timeScale = 1f;
    }


    public int GetCompletionPercent()
    {
        if (totalLevelPages == 0) return 100;
        return Mathf.RoundToInt((float)levelPagesFound / totalLevelPages * 100f);
    }
}