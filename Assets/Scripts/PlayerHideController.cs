using UnityEngine;
using Cainos.PixelArtTopDown_Basic;

public class PlayerHideController : MonoBehaviour
{
    public bool IsHidden { get; private set; }

    private Animator animator;
    private SpriteRenderer sprite;
    private Collider2D col;
    private Rigidbody2D rb;
    private TopDownCharacterController movement;

    void Awake()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<TopDownCharacterController>();
    }

    public void Hide(Transform hidePoint)
    {
        if (IsHidden) return;

        IsHidden = true;

        // Animación del jugador (si existe)
        if (animator)
            animator.SetTrigger("Hide");

        // Bloquear jugador
        movement.enabled = false;
        rb.linearVelocity = Vector2.zero;
        col.enabled = false;

        // Colocar dentro
        transform.position = hidePoint.position;

        // Ocultar sprite
        sprite.enabled = false;
    }

    public void Unhide()
    {
        if (!IsHidden) return;

        IsHidden = false;

        if (animator)
            animator.SetTrigger("Unhide");

        sprite.enabled = true;
        col.enabled = true;
        movement.enabled = true;
    }
}
