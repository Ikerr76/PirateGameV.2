using UnityEngine;

public class TriggerTrap : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        BossHealth boss = other.GetComponent<BossHealth>();
        if (boss != null)
        {
            boss.RecibirDaño(damage);
            Destroy(gameObject);
        }
    }
}
