using UnityEngine;
public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform Point;
    [SerializeField] private LayerMask mask;
    [SerializeField] private LayerMask interactionLayerMask;
    private Player player => GetComponent<Player>();
    private Properties properties => GetComponent<Properties>();
    private Inventory inventory => GetComponent<Inventory>();

    // private
    public InteractiveObject carriedObject = null;
    public Iusable carriedObjectUsableComponent = null;
    public Rigidbody rigidbodyComponent = null;

    void FixedUpdate()
    {
        if (!carriedObject) return;

        Vector3 target = Point.TransformPoint(carriedObject.GetOffset());
        Vector3 delta = target - rigidbodyComponent.position;

        if (delta.sqrMagnitude > properties.GetInteractDistance() * properties.GetInteractDistance())
        {
            DropObject();
            return;
        }

        rigidbodyComponent.rotation = Point.rotation * Quaternion.Euler(carriedObject.GetRotation());
        rigidbodyComponent.linearVelocity = delta * properties.GetPickedUpMoveObjectSpeed();
    }

    public Transform GetPoint() => Point;

    public void CheckAction(float force)
    {
        if (carriedObject == null) TryInteract();
        else DropObject(force);
    }

    public void GetItem(InteractiveObject _object)
    {
        carriedObject = _object;
        rigidbodyComponent = carriedObject.GetComponent<Rigidbody>();
        SetActive();
    }

    public void SetActive()
    {
        carriedObject.setCarried(true);
        carriedObject.ItemLogic(Point,this);
        if (carriedObject.TryGetComponent(out Iusable usable)) carriedObjectUsableComponent = usable;

        rigidbodyComponent.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidbodyComponent.interpolation = RigidbodyInterpolation.Interpolate;
        rigidbodyComponent.linearVelocity = Vector3.zero;
        rigidbodyComponent.freezeRotation = true;
        rigidbodyComponent.isKinematic = false;
        rigidbodyComponent.useGravity = false;
        if(rigidbodyComponent.excludeLayers != mask) rigidbodyComponent.excludeLayers += mask;
        carriedObject.transform.SetParent(null);
    }
    public void Store() => carriedObject.setStored(true);

    public void SetInactive()
    {
        if(!carriedObject) return; 
        carriedObject.setCarried(false);
        carriedObject.ItemLogic();
        carriedObject.setStored(false);
        if (rigidbodyComponent.excludeLayers == mask) rigidbodyComponent.excludeLayers -= mask;
        rigidbodyComponent.useGravity = true;
        rigidbodyComponent.freezeRotation = false;
        rigidbodyComponent.isKinematic = false;
        carriedObjectUsableComponent = null;
    }

    public void DropObject(float force = 0f)
    {
        Vector3 dir = Point ? Point.forward : Vector3.zero;

        SetInactive();

        rigidbodyComponent.AddForce(dir * force, ForceMode.Impulse);

        if (properties != null)
        {
            carriedObject.transform.SetParent(
                properties.chunkManager.chunkQueue[
                    properties.chunkManager.GetPlayerCurrentChunkID() - 1
                ].transform
            );
        }
        carriedObject.setStored(false);
        inventory.RemoveItem(carriedObject.gameObject);
        carriedObject = null;
        carriedObjectUsableComponent = null;
    }

    public void UseItem(Transform PointTransform) => carriedObjectUsableComponent?.Use(PointTransform,properties);
    private void TryInteract()
    {
        Collider raycastHitCollider = new InteractWithObject().GetRaycastHitCollider(player.cameraTransform, properties, interactionLayerMask);
        if (raycastHitCollider && raycastHitCollider.TryGetComponent(out InteractiveObject _object))
        {
            GetItem(_object);
            inventory.AddItem(_object.gameObject);
        }
    }
}
