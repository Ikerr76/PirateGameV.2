using UnityEngine;

public class StaticTrigger : MonoBehaviour
{
    [Header("Trampa estática que se prepara con un ítem")]
    public string requiredPrepItem;   // "Veneno", "Anguila", "Fregona"
    public int damage = 1;

    private bool isPrimed = false;

    public void Prepare(InventoryManager inv)
    {
        if (inv.ContieneItem(requiredPrepItem))
        {
            isPrimed = true;
            Debug.Log($"{name} preparado con {requiredPrepItem}");
        }
        else
        {
            Debug.Log($"Falta {requiredPrepItem} para preparar {name}");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPrimed) return;

        var boss = other.GetComponent<BossHealth>();
        if (boss != null)
        {
            boss.RecibirDaño(damage);
            isPrimed = false;
            Debug.Log($"{name} activado por el boss (-{damage} HP)");
        }
    }
}
