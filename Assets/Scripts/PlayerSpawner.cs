using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject playerPrefab;
    public string spawnAnimationName = "Spawn";

    private GameObject playerInstance;

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        Vector3 fixedPosition = new Vector3(
            spawnPoint.position.x,
            spawnPoint.position.y,
            0  // ← FORZAMOS Z = 0 SIEMPRE
        );

        playerInstance = Instantiate(playerPrefab, fixedPosition, Quaternion.identity);

        // Asignar cámara
        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
            cam.SetTarget(playerInstance.transform);

        // Animación + movimiento
        Animator anim = playerInstance.GetComponent<Animator>();
        var movement = playerInstance.GetComponent<Cainos.PixelArtTopDown_Basic.TopDownCharacterController>();

        if (movement != null)
            movement.enabled = false;

        if (anim != null)
            anim.Play(spawnAnimationName);

        StartCoroutine(EnableMovementAfterSpawn(anim, movement));
    }


    private System.Collections.IEnumerator EnableMovementAfterSpawn(Animator anim, Cainos.PixelArtTopDown_Basic.TopDownCharacterController movement)
    {
        // Esperar duración de animación
        float duration = (anim != null) ? anim.GetCurrentAnimatorStateInfo(0).length : 1f;

        yield return new WaitForSeconds(duration);

        if (movement != null)
            movement.enabled = true;
    }
}
