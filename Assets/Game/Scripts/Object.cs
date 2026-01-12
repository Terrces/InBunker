using System.Collections;
using UnityEngine;

public class Object : MonoBehaviour, Iinteractable, IdropableObject
{
    public enum InteractableObjectType { Object, Item }

    [Header("Hold settings")]
    [SerializeField] private Vector3 localOffset = new Vector3(0, -0.2f, 2f);
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float pickupDelay = 0.15f;

    [Header("Type")]
    [SerializeField] private InteractableObjectType type;

    [Header("Models")]
    [SerializeField] private MeshRenderer rigidBodyModel;
    [SerializeField] private GameObject itemModel;

    private GameObject tempItemModel;

    private Transform point;
    private Interaction interaction;
    private Properties properties;

    private float moveSpeed;
    private float maxDistance;
    private LayerMask excludeLayer;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!point) return;

        Vector3 target = point.TransformPoint(localOffset);
        Vector3 delta = target - rb.position;

        if (delta.sqrMagnitude > maxDistance * maxDistance)
        {
            Drop();
            return;
        }

        rb.rotation = point.rotation * Quaternion.Euler(rotation);
        rb.linearVelocity = delta * moveSpeed;
    }

    public void Interact(InteractionStruct Interaction)
    {
        interaction = Interaction.interactable;
        point = Interaction.pointTransform;
        moveSpeed = Interaction.timeSpeed;
        maxDistance = Interaction.maxDistance;
        properties = Interaction.properties;
        excludeLayer = Interaction.layerMask;

        transform.SetParent(null);

        PrepareRigidbody();
        StartCoroutine(PickupRoutine());
    }

    private IEnumerator PickupRoutine()
    {
        // небольшая пауза — ощущение «поднял»
        yield return new WaitForSeconds(pickupDelay);

        if (type == InteractableObjectType.Item)
        {
            tempItemModel = Instantiate(itemModel, point);
            rigidBodyModel.enabled = false;
        }
    }

    private void PrepareRigidbody()
    {
        rb.linearVelocity = Vector3.zero;
        rb.useGravity = false;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.excludeLayers += excludeLayer;
    }

    public void Drop(float force = 0f)
    {
        Vector3 dir = point ? point.forward : Vector3.zero;
        point = null;

        rb.excludeLayers -= excludeLayer;
        rb.useGravity = true;
        rb.freezeRotation = false;

        if (type == InteractableObjectType.Item)
        {
            rigidBodyModel.enabled = true;
            if (tempItemModel) Destroy(tempItemModel);
        }

        rb.AddForce(dir * force, ForceMode.Impulse);

        interaction?.dropObject();

        if (properties != null)
        {
            transform.SetParent(
                properties.chunkManager.chunkQueue[
                    properties.chunkManager.GetPlayerCurrentChunkID() - 1
                ].transform
            );
        }
    }
}
