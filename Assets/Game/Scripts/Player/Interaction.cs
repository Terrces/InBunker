using UnityEngine;
public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform Point;
    [SerializeField] private LayerMask mask;
    [SerializeField] private LayerMask interactionLayerMask;
    private Player player => GetComponent<Player>();
    private Properties properties => GetComponent<Properties>();

    private Object carriedObject = null;
    public Iusable carriedObjectUsableComponent = null;
    public Transform GetArm() => Point;

    public void CheckAction(float force)
    {
        if (carriedObject == null) TryInteract();
        else
        {
            carriedObject.GetComponent<Object>().Drop(force);
            carriedObject = null;
        }
    }

    public void dropObject()
    {
        carriedObject = null;
        carriedObjectUsableComponent = null;
    }

    public void UseItem(Transform PointTransform) => carriedObjectUsableComponent?.Use(PointTransform,properties);
    private void TryInteract()
    {
        Collider raycastHitCollider = new InteractWithObject().GetRaycastHitCollider(player.cameraTransform, properties, interactionLayerMask);
        if (raycastHitCollider && raycastHitCollider.TryGetComponent(out Iinteractable interactable))
        {    
            interactable.Interact
            (
                new InteractionStruct
                {
                    interactable = this,
                    pointTransform = GetArm(),
                    timeSpeed = properties.GetPickedUpMoveObjectSpeed(), 
                    maxDistance = properties.GetInteractDistance(), 
                    layerMask = mask, 
                    properties = properties
                }
            );
        }
        if (raycastHitCollider && raycastHitCollider.TryGetComponent(out Object _object)) carriedObject = _object;
        if (raycastHitCollider && raycastHitCollider.TryGetComponent(out Iusable usable)) carriedObjectUsableComponent = usable;
    }
}
