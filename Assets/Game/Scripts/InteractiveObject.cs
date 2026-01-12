using System.Collections;
using UnityEngine;

public class InteractiveObject : MonoBehaviour, Iinteractable
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

    private Rigidbody rb;
    private bool isCarried;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody>();
    }
    public Vector3 GetOffset() => localOffset;
    public Vector3 GetRotation() => rotation;
    public InteractableObjectType GetCurrentType() => type;

    public void setCarried(bool carried) => isCarried = carried;

    public void Interact(){}

    public void ItemLogic(Transform point = null)
    {
        if (type == InteractableObjectType.Item && isCarried)
        {
            StartCoroutine(PickupRoutine(point));
        }
        else if(!isCarried)
        {
            if (rigidBodyModel) rigidBodyModel.enabled = true;
            if (tempItemModel) Destroy(tempItemModel);
        }
    }

    private IEnumerator PickupRoutine(Transform point)
    {
        yield return new WaitForSeconds(pickupDelay);

        tempItemModel = Instantiate(itemModel, point);
        rigidBodyModel.enabled = false;
    }
}
