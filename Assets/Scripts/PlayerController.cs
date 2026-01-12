using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 input;
    private Vector2 direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input.Normalize();

        direction = input;

        UpdateAnimator();
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + input * speed * Time.fixedDeltaTime);
    }

    void UpdateAnimator()
    {
        bool moving = direction.magnitude > 0.01f;

        animator.SetBool("IsMoving", moving);

        if (moving)
        {
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
        }
    }
}
