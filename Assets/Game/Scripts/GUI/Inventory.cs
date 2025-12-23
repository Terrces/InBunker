using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<GameObject> slots = new List<GameObject>();
    [SerializeField] Texture transparentImage;
    [SerializeField] Item testItem;
    [Header("Visual")]
    [SerializeField] Vector2 hideInventoryPosition;
    [SerializeField] float tweenDuration;
    Vector3 StartInventoryPosition;
    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        StartInventoryPosition = rect.anchoredPosition;
    }

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

    [ContextMenu("Show Inventory")]
    public void ShowInventory()
    {
        if (StartInventoryPosition != Vector3.zero)
        {
            rect.DOAnchorPos(StartInventoryPosition,tweenDuration).SetEase(Ease.InOutSine);
        }
    }


    [ContextMenu("Hide Inventory")]
    public void HideInventory()
    {
        rect.DOAnchorPos(hideInventoryPosition, tweenDuration).SetEase(Ease.InOutSine);
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
