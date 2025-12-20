using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<GameObject> slots = new List<GameObject>();
    [SerializeField] Item testItem = new Item();
    public void AddItem(int slot)
    {
        Slot slotComponent = slots[slot].GetComponent<Slot>();
        slotComponent.UpdateSlot(testItem.getIcon());
    }
    public void CheckAvailableSlots()
    {
        
    }

    [ContextMenu("test item")]
    public void test()
    {
        AddItem(0);
    }
}
