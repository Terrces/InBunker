using System;
using UnityEngine;

[Serializable]
public class StorageInspector
{
    [Range(0,100)] [SerializeField] private float chance;
    [SerializeField] private GameObject gameObject;
    
    public GameObject getObject() => gameObject;
    public float getChance() => chance;
}
