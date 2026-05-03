using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using System.Collections;


//This class is used to handle the player's health, including taking damage, knockback, and death.
public class PlayerHealth : MonoBehaviour
{
    public int MaxHealth = 10;
    public int currentHealth = 10;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    public Animator animator;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        FindObjectOfType<UIManager>().UpdateHealth(currentHealth, MaxHealth);
        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void Knockback(Vector2 sourcePos, float force)
    {
        Vector2 dir = ((Vector2)transform.position - sourcePos).normalized;
        transform.position += (Vector3)(dir * force);
    }

    IEnumerator DamageFlash()
    {
        sr.color = Color.red;
        FindFirstObjectByType<DamageFlashUI>().Flash();
        yield return new WaitForSeconds(0.5f);
        sr.color = Color.white;
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