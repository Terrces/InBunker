using UnityEngine;
using UnityEngine.UI;

[SelectionBase]
public class Slot : MonoBehaviour
{
    [SerializeField] Item slotItem;
    RawImage image;

    public Item GetSlotItem()
    {
        return slotItem;
    }

    public void UpdateSlot(Item _item)
    {
        image = GetComponentInChildren<RawImage>();
        image.texture = _item.GetIcon();

        slotItem = _item;
    }
    public void ResetItem(Texture texture)
    {
        slotItem = null;

        image = GetComponentInChildren<RawImage>();
        image.texture = texture;
    }
}
