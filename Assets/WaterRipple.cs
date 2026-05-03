using UnityEngine;


//This class defines the behaviour of the ripple ring loop generated when player walks in the waters.
public class RippleLoop : MonoBehaviour
{
    public float minScale = 0.3f;
    public float maxScale = 0.8f;
    public float speed = 2f;
    public float maxAlpha = 0.12f;
    private SpriteRenderer sr;
    private float t;
    public bool isActive = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isActive)
        {
            sr.color = new Color(1,1,1,0);
            return;
        }

        t += Time.deltaTime * speed;
        float wave = Mathf.PingPong(t, 1f);
        float scale = Mathf.Lerp(minScale, maxScale, wave);
        transform.localScale = Vector3.one * scale;
        float alpha = Mathf.Lerp(0f, maxAlpha, wave);
        sr.color = new Color(1, 1, 1, alpha);
    }
}