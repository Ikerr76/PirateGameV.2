using UnityEngine;

public class PrepareStaticPoint : Interactable
{
    public StaticTrigger staticTrap;

    public override void Interact(GameObject actor)
    {
        var inv = actor.GetComponent<InventoryManager>();
        if (inv == null) return;

        staticTrap.Prepare(inv);
    }
}
