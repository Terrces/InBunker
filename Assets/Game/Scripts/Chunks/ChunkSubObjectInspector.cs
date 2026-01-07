using System;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class ChunkSubObjectInspector
{
    public enum axisObjectRandomRotation {Unlock, Both, X, Y, Z} 
    [Range(0,100)]
    public float chance = 100;
    [SerializeField] public GameObject _gameObject;
    public bool DefaultScale = true;
    public bool RandomScale = false;
    public float MinScale = 0.1f;
    public float Scale = 1f;
    public axisObjectRandomRotation lockObjectRotation;
    public bool RandomRotation;
    public Vector3 Rotation;
}
