using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> slots = new List<GameObject>();
    [SerializeField] Texture transparentImage;
    [SerializeField] Item testItem;

    InventoryView inventoryView;

    #region logic

    void Awake() => inventoryView = GetComponent<InventoryView>();

    public void AddItem(int slot)
    {
        Slot slotComponent = slots[slot].GetComponent<Slot>();
        slotComponent.UpdateSlot(testItem);
        inventoryView.ShowInventory();
    }
    public void SubstractItem(int slot)
    {
        Slot slotComponent = slots[slot].GetComponent<Slot>();
        slotComponent.ResetItem(transparentImage);
    }
    public void CheckAvailableSlots()
    {
        
    }
    #endregion

    #region Inventory mechanics tests
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
        inventoryView.HideInventory();
    }

    #endregion
}
