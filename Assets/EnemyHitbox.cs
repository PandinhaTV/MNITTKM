using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    public Enemy enemy; // Reference to main enemy script
    public float damageMultiplier = 1f; // Adjust damage for different hitboxes

    private void Start()
    {
        if (enemy == null)
        {
            enemy = GetComponentInParent<Enemy>(); // Auto-find the main enemy script
        }
    }

    public void RegisterHit(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        enemy.TakeDamage(damage * damageMultiplier, hitPoint, hitNormal);
    }
}