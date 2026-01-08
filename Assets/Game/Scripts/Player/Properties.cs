using UnityEngine;

public class Properties : MonoBehaviour
{
    [Header("Global")]
    public ChunkManager chunkManager;
    [Header("Interaction")]
    [SerializeField] private float maxInteractDistance = 5f;
    [SerializeField] private float PickedUpMoveObjectSpeed = 20f;
    [SerializeField] private float dropForce = 3f;
    public float GetDropForce() => dropForce;
    public float GetInteractDistance() => maxInteractDistance;
    public float GetPickedUpMoveObjectSpeed() => PickedUpMoveObjectSpeed;

}
