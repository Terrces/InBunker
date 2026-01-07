using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] Vector2 hideInventoryPosition;
    [SerializeField] Ease animationEase = Ease.Linear;
    [SerializeField] float tweenDuration;
    [SerializeField] float durationForHidingInventory = 2;

    Vector2 StartInventoryPosition;
    private RectTransform rect;
    private List<RectTransform> slotsRects = new();
    
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        StartInventoryPosition = rect.anchoredPosition;
        // foreach(GameObject slot in GetComponent<Inventory>().slots) slotsRects.Add(slot.GetComponent<RectTransform>());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(durationForHidingInventory);
        HideInventory();
    }

    void Start() => HideInventory();

    [ContextMenu("Show Inventory")]
    public void ShowInventory()
    { 
        PlayTweenAnimation(StartInventoryPosition.y);
        StartCoroutine(Timer());
    }
    
    [ContextMenu("Hide Inventory")]
    public void HideInventory() => PlayTweenAnimation(hideInventoryPosition.y, 0.05f);

    private void PlayTweenAnimation(float endPositionY,float additionalDuration = 0.1f)
    {
        float _additionalDuration = 0f;
        foreach (RectTransform _rect in slotsRects)
        {
            _rect.DOAnchorPosY(endPositionY, tweenDuration + _additionalDuration).SetEase(animationEase);
            _additionalDuration += additionalDuration;
        }
    }
}
