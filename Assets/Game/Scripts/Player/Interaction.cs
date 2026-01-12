using UnityEngine;
public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform Point;
    [SerializeField] private LayerMask mask;
    [SerializeField] private LayerMask interactionLayerMask;
    private Player player => GetComponent<Player>();
    private Properties properties => GetComponent<Properties>();

    private InteractiveObject carriedObject = null;
    public Iusable carriedObjectUsableComponent = null;
    public Rigidbody rigidbodyComponent = null;
    // private 

    void FixedUpdate()
    {
        if (!carriedObject) return;

        Vector3 target = Point.TransformPoint(carriedObject.GetOffset());
        Vector3 delta = target - rigidbodyComponent.position;

        if (delta.sqrMagnitude > properties.GetInteractDistance() * properties.GetInteractDistance())
        {
            dropObject();
            return;
        }

        rigidbodyComponent.rotation = Point.rotation * Quaternion.Euler(carriedObject.GetRotation());
        rigidbodyComponent.linearVelocity = delta * properties.GetPickedUpMoveObjectSpeed();
    }

    public void CheckAction(float force)
    {
        if (carriedObject == null) TryInteract();
        else dropObject(force);
    }

    private void dropObject(float force = 0f)
    {
        Vector3 dir = Point ? Point.forward : Vector3.zero;

        rigidbodyComponent.excludeLayers -= mask;
        rigidbodyComponent.useGravity = true;
        rigidbodyComponent.freezeRotation = false;
        carriedObject.setCarried(false);
        carriedObject.ItemLogic();

        rigidbodyComponent.AddForce(dir * force, ForceMode.Impulse);

        if (properties != null)
        {
            carriedObject.transform.SetParent(
                properties.chunkManager.chunkQueue[
                    properties.chunkManager.GetPlayerCurrentChunkID() - 1
                ].transform
            );
        }

        carriedObject = null;
        carriedObjectUsableComponent = null;
    }

    public void UseItem(Transform PointTransform) => carriedObjectUsableComponent?.Use(PointTransform,properties);
    private void TryInteract()
    {
        Collider raycastHitCollider = new InteractWithObject().GetRaycastHitCollider(player.cameraTransform, properties, interactionLayerMask);
        if (raycastHitCollider && raycastHitCollider.TryGetComponent(out Iinteractable interactable)) interactable.Interact();
        if (raycastHitCollider && raycastHitCollider.TryGetComponent(out InteractiveObject _object))
        {
            carriedObject = _object;
            rigidbodyComponent = carriedObject.GetComponent<Rigidbody>();

            carriedObject.setCarried(true);
            carriedObject.ItemLogic(Point);
            
            rigidbodyComponent.linearVelocity = Vector3.zero;
            rigidbodyComponent.useGravity = false;
            rigidbodyComponent.freezeRotation = true;
            rigidbodyComponent.interpolation = RigidbodyInterpolation.Interpolate;
            rigidbodyComponent.collisionDetectionMode = CollisionDetectionMode.Continuous;
            rigidbodyComponent.excludeLayers += mask;
        }
        if (raycastHitCollider && raycastHitCollider.TryGetComponent(out Iusable usable)) carriedObjectUsableComponent = usable;
    }
}
