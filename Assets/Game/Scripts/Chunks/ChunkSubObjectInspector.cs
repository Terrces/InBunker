using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChunkSubObjectInspector
{
    public enum axisObjectRandomRotation {Unlock, Both, X, Y, Z} 
    [Range(0,100)]
    public float chance = 100;
    [SerializeField] public GameObject _gameObject;
    public List<StorageInspector> storage;
    public bool DefaultMass = true;
    public bool DefaultScale = true;
    public bool RandomMass = false;
    public bool RandomScale = false;
    public float MinScale = 1f;
    public float Scale = 1f;
    public float MinMass = 1f;
    public float Mass = 1f;
    public axisObjectRandomRotation lockObjectRotation;
    public bool RandomRotation;
    public Vector3 Rotation;
}
