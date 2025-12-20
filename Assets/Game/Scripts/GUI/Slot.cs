using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] RawImage image;

    public void UpdateSlot(Texture icon)
    {
        image.texture = icon;
    }
}
