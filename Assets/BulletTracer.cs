using UnityEngine;


//This class is used to create a bullet tracer effect for the player's attacks, which visually represents the path of the bullet and adds to the feedback and immersion of the combat.
public class BulletTracer : MonoBehaviour
{
    public float speed = 25f;

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }

    void Start()
    {
        Destroy(gameObject, 0.15f);
    }
}