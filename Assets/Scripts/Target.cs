using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public GameObject target;
    public Animator animator;
    public string dieAnimationTrigger = "DieTrigger";

    private DeerMovement deerMovement;
    public bool isDead = false;

    private void Start()
    {
        // Get the DeerMovement component from the target GameObject
        deerMovement = target.GetComponent<DeerMovement>();

        isDead = false;
        if (health <= 0)
        {
            health = 50f;
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        deerMovement.isDead = true; // Set isDead in DeerMovement script to true
        isDead = true;
        animator.SetBool("isDead", true);

        // Find the DeerManager component dynamically
        DeerManager deerManager = FindObjectOfType<DeerManager>();
        if (deerManager != null)
        {
            deerManager.IncrementDeadDeers();
        }
        else
        {
            Debug.LogError("DeerManager not found in the scene.");
        }
    }
}
