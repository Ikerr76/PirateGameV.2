using UnityEngine;

public class HideSpot : MonoBehaviour
{
    public Animator armarioAnimator;

    private bool ocupado = false;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!Input.GetKeyDown(KeyCode.E)) return;

        var hide = other.GetComponent<PlayerHideController>();
        if (hide == null) return;

        if (!ocupado)
        {
            ocupado = true;
            armarioAnimator?.SetTrigger("Open");
            hide.Hide(transform);
        }
        else
        {
            ocupado = false;
            armarioAnimator?.SetTrigger("Close");
            hide.Unhide();
        }
    }
}
