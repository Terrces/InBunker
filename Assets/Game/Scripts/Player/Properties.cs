using UnityEngine;

public class Properties : MonoBehaviour
{
    [Header("Global")]
    public ChunkManager chunkManager;
    [Header("Stats")]
    public bool Died = false;
    public int health = 100;
    public int Temperature = 100;
    [Header("Interaction")]
    public Interaction Interaction => GetComponent<Interaction>();
    [SerializeField] private float maxInteractDistance = 5f;
    [SerializeField] private float PickedUpMoveObjectSpeed = 20f;
    [SerializeField] private float dropForce = 3f;
    public float GetDropForce() => dropForce;
    public float GetInteractDistance() => maxInteractDistance;
    public float GetPickedUpMoveObjectSpeed() => PickedUpMoveObjectSpeed;
    
    public bool AddHealth(int value)
    {
        if(health < 100)
        {
            health += value;
            return true;
        }
        
        return false;
    }
}
