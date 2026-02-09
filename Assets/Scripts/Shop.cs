using LotG.Inventories;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] InventoryItem[] items;
    [SerializeField] float timeToCollect = 10f;

    PlayerControls controls;
    Inventory playerInventory;
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
        playerInventory = FindFirstObjectByType<Inventory>();
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
        foreach (InventoryItem item in items)
        {
            playerInventory.AddToFirstEmptySlot(item, 1);
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
