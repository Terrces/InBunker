using UnityEngine;

[System.Serializable]
public class Item
{
    enum Types {}
    [SerializeField] string itemName;
    [SerializeField] Texture icon;
    public Texture GetIcon() => icon;
    public string GetName() => itemName; 
    public void SetIcon(Texture _icon) => icon = _icon;
}
