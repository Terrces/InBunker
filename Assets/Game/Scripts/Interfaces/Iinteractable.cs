using UnityEngine;
public interface Iinteractable
{
    public void Interact(Interaction iinteractable, Transform handsTransform, float timeSpeed,float maxDistance, LayerMask layerMask);
}
