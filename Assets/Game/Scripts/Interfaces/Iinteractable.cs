using UnityEngine;
public interface Iinteractable
{
    public void Interact(Interaction interactable, Transform pointTransform, float timeSpeed, float maxDistance, LayerMask layerMask, Properties properties);
}
