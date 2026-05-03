using UnityEngine;


//This class is used to create a camera shake effect based on the player's sanity level, adding to the atmosphere and tension of the game as the player's sanity decreases.
public class CameraSanityShake : MonoBehaviour
{
    [Header("References")]
    public Transform target;          
    public PlayerSanity PlayerSanity;

    [Header("Follow")]
    public float smoothSpeed = 8f;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Shake")]
    public float maxShakeAmount = 0.25f;   
    public float shakeSpeed = 25f;

    private Vector3 currentPos;

    void Start()
    {
        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }

        if (PlayerSanity == null)
            PlayerSanity = FindFirstObjectByType<PlayerSanity>();
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;
        currentPos = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smoothSpeed);
        float sanityPercent = 1f;

        if (PlayerSanity != null)
            sanityPercent = PlayerSanity.currentSanity / PlayerSanity.maxSanity;

        
        float shakeStrength = (1f - sanityPercent) * maxShakeAmount;

        Vector3 shakeOffset = new Vector3(
            Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f,
            Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f,
            0f
        ) * shakeStrength * 2f;

        transform.position = currentPos + shakeOffset;
    }
}