using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform armPoint;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float smoothTime = 20f;
    [SerializeField] private LayerMask mask;
    private Player player => GetComponent<Player>();
    private Properties properties => GetComponent<Properties>();

    public IdropableObject carriedObject = null;
    public Transform GetArm() => armPoint;

    public void CheckAction(float force)
    {
        if (carriedObject == null) TryInteract();
        else 
        {
            carriedObject.OnDrop(force, properties);
            carriedObject = null;
        }
    }

    private void TryInteract()
    {
        Ray ray = new Ray(player.cameraTransform.position, player.cameraTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {

            if (hit.collider.TryGetComponent(out Iinteractable interactable)) interactable.Interact(this, GetArm(), smoothTime, mask);
            if (hit.collider.TryGetComponent(out IdropableObject dropable)) carriedObject = dropable;
        }
    }
}
