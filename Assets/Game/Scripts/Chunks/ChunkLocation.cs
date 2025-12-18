using UnityEngine;

[SelectionBase]
public class ChunkLocation : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool enableLocationRotation;
    [SerializeField] private Vector3 rotationRadius;

    public Vector3 GetOffset() => offset;
    public bool GetLocationRotating() => enableLocationRotation;
    public Vector3 GetRotationRadius() => rotationRadius;
}
