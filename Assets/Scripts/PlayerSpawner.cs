using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public BossController bossController;

    [Header("Player UI Slots")]
    public Image slot1Image;
    public Image slot2Image;
    public TextMeshProUGUI mensajeTMP;

    private GameObject playerInstance;

    void Start()
    {
        Camera.main.transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            Camera.main.transform.position.z
        );

        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        Vector3 fixedPos = new Vector3(
            transform.position.x,
            transform.position.y,
            0
        );

        playerInstance = Instantiate(playerPrefab, fixedPos, Quaternion.identity);

        CameraFollow cam = Camera.main.GetComponent<CameraFollow>();
        if (cam != null)
            cam.SetTarget(playerInstance.transform);

        InventoryManager inv = playerInstance.GetComponent<InventoryManager>();
        inv.slot1Image = slot1Image;
        inv.slot2Image = slot2Image;
        inv.mensajeTMP = mensajeTMP;

        // <= Cambiado para usar BossController
        if (bossController != null)
            bossController.SetPlayer(playerInstance.transform);
    }
}
