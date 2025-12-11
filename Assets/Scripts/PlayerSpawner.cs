using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public EnemyPathController enemyPathController;

    [Header("Player UI Slots")]
    public Image slot1Image;
    public Image slot2Image;
    public TextMeshProUGUI mensajeTMP;

    private GameObject playerInstance;

    void Start()
    {
        // SpawnPlayer();   // ← AHORA SIEMPRE SPAWNEA

        // Recoloca la cámara en el punto de spawn
        Camera.main.transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            Camera.main.transform.position.z
        );
    }

    private void SpawnPlayer()
    {
        Vector3 fixedPosition = new Vector3(
            transform.position.x,
            transform.position.y,
            0
        );

        playerInstance = Instantiate(playerPrefab, fixedPosition, Quaternion.identity);

        // Cámara sigue al jugador
        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
            cam.SetTarget(playerInstance.transform);

        // Configurar inventario
        InventoryManager inventory = playerInstance.GetComponent<InventoryManager>();
        inventory.slot1Image = slot1Image;
        inventory.slot2Image = slot2Image;
        inventory.mensajeTMP = mensajeTMP;

        // decirle al jefe dónde está el jugador
        enemyPathController.SetPlayer(playerInstance.transform);
    }
}
