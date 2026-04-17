using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyKill : MonoBehaviour
{

    public int damage = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject
                .GetComponent<PlayerHealth>()
                .TakeDamage(damage);
        }
    }
}