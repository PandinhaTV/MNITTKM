using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float health = 100f;

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
        Destroy(gameObject); // Remove enemy
    }
}
