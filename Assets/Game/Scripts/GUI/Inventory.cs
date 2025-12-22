using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<GameObject> slots = new List<GameObject>();
    [SerializeField] Texture transparentImage;
    [SerializeField] Item testItem;
    public void AddItem(int slot)
    {
        Slot slotComponent = slots[slot].GetComponent<Slot>();
        slotComponent.UpdateSlot(testItem);
    }
    public void SubstractItem(int slot)
    {
        Slot slotComponent = slots[slot].GetComponent<Slot>();
        // slotComponent.UpdateSlot(nullItem);
        slotComponent.ResetItem(transparentImage);
    }
    public void CheckAvailableSlots()
    {
        
    }

    [ContextMenu("test item")]
    public void test()
    {
        AddItem(0);
    }

    [ContextMenu("Reset Inventory")]
    public void ResetInventory()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            SubstractItem(i);
        }
    } 
}
