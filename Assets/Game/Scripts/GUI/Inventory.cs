using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> slots = new List<GameObject>();
    [SerializeField] Texture transparentImage;
    [SerializeField] ItemsList itemsList;
    InventoryView inventoryView;

    void Awake() => inventoryView = GetComponent<InventoryView>();

    public void AddItem(int slot, Item item)
    {
        Slot slotComponent = slots[slot].GetComponent<Slot>();
        slotComponent.UpdateSlot(item);
        inventoryView.ShowInventory();
    }
    public void DestroyItem(int slot)
    {
        Slot slotComponent = slots[slot].GetComponent<Slot>();
        slotComponent.ResetItem(transparentImage);
    }
    public void DropItem(int slot)
    {
        Slot slotComponent = slots[slot].GetComponent<Slot>();
        Item slotItem = slotComponent.GetSlotItem();
        if (slotItem != null && slotItem.GetDroppable() && slotItem.GetDroppedObject())
        {
            Debug.Log("Item Dropped");
        }
    }
    public void CheckAvailableSlots()
    {
        
    }

    
    [ContextMenu("Add First Item from list")]
    public void AddFirstItemFromList()
    {
        if (!itemsList) return;
        AddItem(0,itemsList.GetItemByIndex(0));
    }

    [ContextMenu("Reset Inventory")]
    public void ResetInventory()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            DestroyItem(i);
        }
        inventoryView.HideInventory();
    }

}
