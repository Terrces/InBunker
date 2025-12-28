using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField] private Transform armPoint;
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private LayerMask obstacleMask;

    public IdropableObject carriedObject;
    private Player player;
    [SerializeField] float AddictionalZ;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        if (carriedObject != null)
        {
            UpdateCarriedObjectPosition();
        }
    }

    public GameObject GetArm() => armPoint.gameObject;

    public void CheckAction(float force)
    {
        if (carriedObject == null)
            TryInteract();
        else
            Drop(force);
    }

    private void TryInteract()
    {
        Ray ray = new Ray(player.cameraTransform.position, player.cameraTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
        {
            if (hit.collider.TryGetComponent(out IdropableObject dropable))
                carriedObject = dropable;

            if (hit.collider.TryGetComponent(out Iinteractable interactable))
                interactable.Interact(this);
        }
    }

    private void Drop(float force)
    {
        carriedObject.OnDrop(force);
        carriedObject = null;
    }

    private void UpdateCarriedObjectPosition()
    {
        Vector3 cameraPos = player.cameraTransform.position;
        // Целевая точка — это позиция рук + твой AddictionalZ
        Vector3 targetWorldPos = armPoint.position + (armPoint.forward * AddictionalZ);
        
        Vector3 dir = targetWorldPos - cameraPos;
        float distance = dir.magnitude;

        // Размер куба (подбери под свои объекты)
        Vector3 halfExtents = new Vector3(0.1f, 0.1f, 0.1f); 

        // Пускаем BoxCast
        if (Physics.BoxCast(cameraPos, halfExtents, dir.normalized, out RaycastHit hit, armPoint.rotation, distance, obstacleMask))
        {
            Vector3 boxStopPos = cameraPos + dir.normalized * hit.distance;
            carriedObject.SetTargetPosition(boxStopPos, false);
        }
        else
        {
            carriedObject.SetTargetPosition(Vector3.zero, true);
        }
    }
}
