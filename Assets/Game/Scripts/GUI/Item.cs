using System;
using UnityEngine;

[Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] Texture icon;
    public Texture getIcon() => icon;
}
