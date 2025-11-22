using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Punto de spawn del jugador")]
    public Transform spawnPoint;

    [Header("Jugador")]
    public GameObject playerPrefab;

    [Header("Animación de spawn")]
    public string spawnAnimationName = "Spawn";

    private GameObject playerInstance;

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        // Instanciar jugador
        playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);

        // Reproducir animación
        Animator anim = playerInstance.GetComponent<Animator>();
        if (anim != null && !string.IsNullOrEmpty(spawnAnimationName))
        {
            anim.Play(spawnAnimationName);
        }

        // Desactivar movimiento hasta que termine la animación
        var movement = playerInstance.GetComponent<Cainos.PixelArtTopDown_Basic.TopDownCharacterController>();
        if (movement != null)
            movement.enabled = false;

        // Rehabilitar movimiento cuando la animación termine
        StartCoroutine(EnableMovementAfterAnimation(anim));
    }

    private System.Collections.IEnumerator EnableMovementAfterAnimation(Animator anim)
    {
        if (anim != null)
        {
            // Esperar a que termine la animación actual
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }

        // Activar movimiento
        var movement = playerInstance.GetComponent<Cainos.PixelArtTopDown_Basic.TopDownCharacterController>();
        if (movement != null)
            movement.enabled = true;
    }
}
