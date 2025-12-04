using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    //public Transform spawnPoint;
    public GameObject playerPrefab;
    public EnemyPathController enemyPathController;
    //public string spawnAnimationName = "Spawn";

    [Header("Player UI Slots")]
    public Image slot1Image;
    public Image slot2Image;
    public TextMeshProUGUI mensajeTMP;

    private GameObject playerInstance;

    void Start()
    {
        //StartSpawn();
        Camera.main.transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            Camera.main.transform.position.z
        );
    }

    //void StartSpawn()
    //{
    //    //Vector3 fixedPosition = new Vector3(
    //    //    spawnPoint.position.x,
    //    //    spawnPoint.position.y,
    //    //    0  // ← FORZAMOS Z = 0 SIEMPRE
    //    //);

    //}

    private void SpawnPlayer()
    {
        Vector3 fixedPosition = new Vector3(
            transform.position.x,
            transform.position.y,
            0  // ← FORZAMOS Z = 0 SIEMPRE
        );

        playerInstance = Instantiate(playerPrefab, fixedPosition, Quaternion.identity);

        // Asignar cámara
        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
            cam.SetTarget(playerInstance.transform);

        // Animación + movimiento
        //Animator anim = playerInstance.GetComponent<Animator>();
        //var movement = playerInstance.GetComponent<Cainos.PixelArtTopDown_Basic.TopDownCharacterController>();

        //if (movement != null)
        //    movement.enabled = false;

        //if (anim != null)
        //    anim.Play(spawnAnimationName);

        InventoryManager inventory = playerInstance.GetComponent<InventoryManager>();
        inventory.slot1Image = slot1Image;
        inventory.slot2Image = slot2Image;
        inventory.mensajeTMP = mensajeTMP;

        //StartCoroutine(EnableMovementAfterSpawn(anim, movement));
        enemyPathController.Initialize(playerInstance.transform);
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
