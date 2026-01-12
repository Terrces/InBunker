using UnityEngine;

public struct InteractionStruct
{
    public Interaction interactable;
    public Transform pointTransform;
    public Properties properties;
    public LayerMask layerMask;
    public float maxDistance;
    public float timeSpeed;
}
public interface Iinteractable
{
    public void Interact(InteractionStruct parameters);
}
