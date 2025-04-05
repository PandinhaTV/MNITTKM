using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;
    private Animator animator;

    private void Start()
    {
        // Get all Rigidbodies and Colliders in the ragdoll
        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        
        animator = GetComponent<Animator>();

        // Disable ragdoll physics at start
        SetRagdollState(false);
    }

    public void TakeDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;
        Debug.Log("Enemy took damage! Health: " + health);

        if (health <= 0)
        {
            Die(hitPoint, hitNormal);
        }
    }

    void Die(Vector3 hitPoint, Vector3 hitNormal)
    {
        Debug.Log("Enemy died!");
        SetRagdollState(true); // Enable ragdoll
        Destroy(gameObject, 50f); // Remove after delay
        ApplyForceToRagdoll(hitPoint, hitNormal);
    }

    void SetRagdollState(bool isRagdoll)
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = !isRagdoll; // Enable physics on death
        }

        

        if (animator != null)
        {
            animator.enabled = !isRagdoll; // Disable animator when ragdoll is active
        }
    }

    void ApplyForceToRagdoll(Vector3 hitPoint, Vector3 hitNormal)
    {
        foreach (Rigidbody rb in ragdollBodies)
        {
            float forceAmount = 5f; // Adjust force as needed

            // Find the closest ragdoll part to the hit point
            float closestDistance = Mathf.Infinity;
            Rigidbody closestBody = null;

            foreach (Rigidbody part in ragdollBodies)
            {
                float distance = Vector3.Distance(part.position, hitPoint);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestBody = part;
                }
            }

            // Apply force to the closest body part
            if (closestBody != null)
            {
                closestBody.AddForceAtPosition(hitNormal * -forceAmount, hitPoint, ForceMode.Impulse);
            }
        }
    }
}
