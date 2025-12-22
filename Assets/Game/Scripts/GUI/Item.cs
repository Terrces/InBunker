using UnityEngine;

[System.Serializable]
public class Item
{
    [SerializeField] Texture icon;
    public Texture getIcon() => icon;
    public void setIcon(Texture _icon) => icon = _icon;
}
