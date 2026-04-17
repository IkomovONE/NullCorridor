using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 2f;
    public float range = 3f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.position = startPos + new Vector3(Mathf.PingPong(Time.time * speed, range), 0, 0);
    }
}