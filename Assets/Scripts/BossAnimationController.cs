using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    public BossController bossController;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    void Update()
    {
        if (bossController == null || animator == null || spriteRenderer == null)
            return;

        Vector2 dir = bossController.Direction;

        animator.SetFloat("MoveX", dir.x);
        animator.SetFloat("MoveY", dir.y);

        animator.SetBool("IsMoving", dir.magnitude > 0.01f);

        if (Mathf.Abs(dir.x) > 0.1f)
            spriteRenderer.flipX = dir.x < 0;
    }
}
