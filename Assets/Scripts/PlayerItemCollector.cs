using UnityEngine;

public class PlayerItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryController = FindObjectOfType<InventoryController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                // Show pickup notification before adding to inventory
                item.PickUp();
                
                // Add item to inventory
                bool itemAdded = inventoryController.AddItem(item.gameObject);
                if (itemAdded)
                {
                    Destroy(collision.gameObject);
                }
            }
        }
    }
}