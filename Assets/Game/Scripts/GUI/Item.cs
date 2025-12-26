using UnityEngine;

[System.Serializable]
public class Item
{
    public enum ItemTypes {unset,food,tool}
    [SerializeField] string itemName;
    [SerializeField] string itemDescription;
    [SerializeField] Texture icon;
    [SerializeField] int maxStack = 1;
    [SerializeField] ItemTypes type = ItemTypes.unset;
    [SerializeField] GameObject droppedObject;
    [SerializeField] bool droppable;

    public Texture GetIcon() => icon;
    public string GetName() => itemName; 
    public string GetDescription() => itemName; 
    public int GetMaxStack() => maxStack; 
    public ItemTypes GetItemType() => type; 
    public GameObject GetDroppedObject() => droppedObject;
    public bool GetDroppable() => droppable; 

}
