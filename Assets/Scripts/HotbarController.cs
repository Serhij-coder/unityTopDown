using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class HotbarController : MonoBehaviour
{
    public GameObject hotbarPanel; public GameObject slotPrefab;
    public int slotCount = 10; //1-0 on the keyboard
    private ItemDictionary itemDictionary;
    private Key[] hotbarkeys;


    private void Awake()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();
        // Hotbar keys based on slot count
        hotbarkeys = new Key[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            hotbarkeys[i] = i < 9 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for key presses
        for (int i = 0; i < slotCount; i++)
        {
            if (Keyboard.current[hotbarkeys[i]].wasPressedThisFrame)
            {
                //UseItem
                UseItemInSlot(i);
            }
        }
    }

    void UseItemInSlot(int index)
    {
        Slot slot = hotbarPanel.transform.GetChild(index).GetComponent<Slot>();
        if (slot.currentItem != null)
        {
            Item item = slot.currentItem.GetComponent<Item>();
            item.UseItem();
        }
    }

    public List<InventorySaveData> GetHotbarItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach (Transform slotTransform in hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if (item != null)
                {
                    invData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
                }
            }
        }
        return invData;
    }

    public void SetHotbarItems(List<InventorySaveData> inventorySaveData)
    {
        // Clear inventory panel - avoid duplicates
        foreach (Transform child in hotbarPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Create new slots
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, hotbarPanel.transform);
        }

        // Populate slots with saved items
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = hotbarPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                if (itemDictionary != null)
                {
                    GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                    if (itemPrefab != null)
                    {
                        GameObject item = Instantiate(itemPrefab, slot.transform);
                        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                        slot.currentItem = item;
                    }
                }
            }
        }
    }
}