using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] Item slotItem;
    RawImage image;

    public void UpdateSlot(Item _item)
    {
        image = GetComponentInChildren<RawImage>();
        image.texture = _item.getIcon();

        slotItem = _item;
    }
    public void ResetItem(Texture texture)
    {
        slotItem = null;

        image = GetComponentInChildren<RawImage>();
        image.texture = texture;
    }
}
