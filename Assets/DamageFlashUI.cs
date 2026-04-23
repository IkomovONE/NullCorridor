using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageFlashUI : MonoBehaviour
{
    public Image flashImage;

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        flashImage.color = new Color(1f, 0f, 0f, 0.25f);

        yield return new WaitForSeconds(0.08f);

        float t = 0f;

        while (t < 0.3f)
        {
            t += Time.deltaTime;

            float a = Mathf.Lerp(0.35f, 0f, t / 0.3f);
            flashImage.color = new Color(1f, 0f, 0f, a);

            yield return null;
        }

        flashImage.color = new Color(1f, 0f, 0f, 0f);
    }
}