using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Image healthFill;
    public Image staminaFill;
    public Image sanityFill;

    public TMP_Text ammoText;

    public int diaryPages = 0;

    public TMP_Text popupText;
    public GameObject popupPanel;

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

        if (popupPanel != null)
            popupPanel.SetActive(false);
    }

    void Update()
    {
        healthFill.fillAmount =
            Mathf.Lerp(
                healthFill.fillAmount,
                targetHealth,
                Time.deltaTime * smoothSpeed
            );

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

    public void CollectDiary(string message)
    {
        diaryPages++;

        PlayerPrefs.SetInt("DiaryPages", diaryPages);

        StartCoroutine(ShowPopup("+1 DIARY PAGE FOUND"));

        ShowDiary(message);
    }

    IEnumerator ShowPopup(string msg)
    {
        popupPanel.SetActive(true);

        popupText.text = msg;

        yield return new WaitForSecondsRealtime(2f);

        popupPanel.SetActive(false);
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
}