using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform Point;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float smoothTime = 20f;
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
            carriedObject.GetComponent<Object>().Drop(force, properties);
            carriedObject = null;
        }
    }

    public void dropObject()
    {
        carriedObject = null;
        carriedObjectUsableComponent = null;
    }

    public void UseItem()
    {
        if (carriedObjectUsableComponent != null) carriedObjectUsableComponent.Use();
    }
    private void TryInteract()
    {
        Ray ray = new Ray(player.cameraTransform.position, player.cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance,interactionLayerMask))
        {
            if (hit.collider.TryGetComponent(out Iinteractable interactable)) interactable.Interact(this, GetArm(), smoothTime, maxDistance, mask);
            if (hit.collider.TryGetComponent(out Object _object)) carriedObject = _object;
            if (hit.collider.TryGetComponent(out Iusable usable)) carriedObjectUsableComponent = usable;
        }
    }
}
