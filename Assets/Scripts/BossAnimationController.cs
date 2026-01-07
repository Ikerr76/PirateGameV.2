using System;
using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    [SerializeField] private Animator bossAnimator;
    [SerializeField] private SpriteRenderer bossSpriteRenderer;
    [SerializeField] private EnemyPathController EnemypathController;

    private void Start()
    {
        EnemypathController.onPointReach += UpdateDirection;
    }

    private void UpdateDirection()
    {
        //bossSpriteRenderer.flipX = pathController.Direction.x < 0;
        bossSpriteRenderer.transform.localScale = new Vector3(
           EnemypathController.Direction.x < 0 ? -1 : 1,
            bossSpriteRenderer.transform.localScale.y,
            bossSpriteRenderer.transform.localScale.z
        );
    }
}
