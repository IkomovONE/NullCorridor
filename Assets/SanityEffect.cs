using UnityEngine;
using UnityEngine.UI;


//This class handles the brighness overlay used to represent the sanity effect on the screen.
public class SanityDarknessUI : MonoBehaviour
{
    public Image darknessImage;
    public PlayerSanity sanity;

    public float maxAlpha = 0.65f;

    void Update()
    {
        if (sanity == null || darknessImage == null) return;

        float percent = sanity.currentSanity / sanity.maxSanity;
        float alpha = (1f - percent) * maxAlpha;
        Color c = darknessImage.color;
        c.a = alpha;
        darknessImage.color = c;
    }
}