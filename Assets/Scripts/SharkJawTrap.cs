using UnityEngine;

public class SharkJawTrap : MonoBehaviour
{
    public float pauseTime = 5f;
    public int damage = 0;
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        EnemyPathController boss = other.GetComponent<EnemyPathController>();
        BossHealth bossHealth = other.GetComponent<BossHealth>();

        if (boss != null)
        {
            activated = true;

            // Pausar boss
            boss.PausarPorTrampa(pauseTime);

            // Daño opcional
            if (bossHealth != null && damage > 0)
                bossHealth.RecibirDaño(damage);

            Debug.Log("🦈 Trampa de mandíbula activada");

            // Si es de un solo uso
            Destroy(gameObject, 0.1f);
        }
    }
}
