using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
     public Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Health: " + currentHealth);

        animator.SetInteger("Animate", 1);
        GetComponent<PlayerMovement>().LockAnimation(1f);
        Invoke("ReturnIdle", 1f);

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    void ReturnIdle()
    {
        animator.SetInteger("Animate", 0);
    }
}