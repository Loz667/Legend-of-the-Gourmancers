using LotG.Inventories;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] InventoryItemSO[] items;
    [SerializeField] float timeToCollect = 10f;
    [SerializeField] InventorySO playerInventory;

    PlayerControls controls;
    PlayerInteractUI playerInteractUI;

    float collectionTimer;
    bool collectionAvailable = false;
    bool playerInRange = false;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void Start()
    {
        playerInteractUI = FindFirstObjectByType<PlayerInteractUI>();
    }

    private void Update()
    {
        if (playerInRange && collectionAvailable)
        {
            playerInteractUI.ShowInteractUI();

            if (controls.Player.Interact.WasPressedThisFrame())
            {
                CollectResources();
            }
        }
        else
        {
            playerInteractUI.HideInteractUI();
        }

        if (collectionAvailable) return;

        collectionTimer += Time.deltaTime;
        if (collectionTimer >= timeToCollect)
        {
            Debug.Log("Resources available!");
            collectionTimer = timeToCollect;
            collectionAvailable = true;         
        }
    }

    private void CollectResources()
    {
        foreach (InventoryItemSO item in items)
        {
            playerInventory.AddItem(item, 1);
        }
        collectionAvailable = false;
        collectionTimer = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
    }
}
