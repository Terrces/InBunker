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
    [SerializeField] Ease animationEase;
    [SerializeField] float tweenDuration;
    Vector3 StartInventoryPosition;
    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        StartInventoryPosition = rect.anchoredPosition;

    }
    void Start()
    {
        HideInventory();
    }
    
    // Inventory logic

    public void AddItem(int slot)
    {
        Slot slotComponent = slots[slot].GetComponent<Slot>();
        slotComponent.UpdateSlot(testItem);
    }
    public void SubstractItem(int slot)
    {
        Slot slotComponent = slots[slot].GetComponent<Slot>();
        slotComponent.ResetItem(transparentImage);
    }
    public void CheckAvailableSlots()
    {
        
    }

    // Visual logic
    
    [ContextMenu("Show Inventory")]
    public void ShowInventory() => tweenAnimation(StartInventoryPosition.y);
    
    [ContextMenu("Hide Inventory")]
    public void HideInventory() => tweenAnimation(hideInventoryPosition.y, 0.05f);

    private void tweenAnimation(float endPositionY,float additionalDuration = 0.1f)
    {
        float _additionalDuration = 0f;
        foreach (GameObject obj in slots)
        {
            RectTransform _rect = obj.GetComponent<RectTransform>();
            _rect.DOAnchorPosY(endPositionY, tweenDuration + _additionalDuration).SetEase(animationEase);
            _additionalDuration += additionalDuration;
        }
    }

    // Functions for testing mechanics

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
