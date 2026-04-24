using UnityEngine;

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