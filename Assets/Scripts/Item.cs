using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public int ID;
    public string name;

    public virtual void PickUp()
    {
        Sprite itemIcon = GetComponent<Image>().sprite;
        if (ItemPickupUIController.Instance != null)
        {
            ItemPickupUIController.Instance.ShowItemPickup(name, itemIcon);
        }
    }

    public virtual void UseItem()
    {
        // Base implementation - override in derived classes for specific item behavior
        Debug.Log($"Using item: {name}");
    }
}