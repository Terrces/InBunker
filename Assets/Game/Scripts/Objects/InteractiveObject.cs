using System.Collections;
using NUnit.Framework;
using UnityEngine;

public class InteractiveObject : MonoBehaviour, IliftedObject
{
    public enum InteractableObjectType { Object, Item }

    [Header("Hold settings")]
    [SerializeField] private Vector3 localOffset = new Vector3(0, -0.2f, 0f);
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
    private bool isStored;
    public Interaction interaction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb) rb = gameObject.AddComponent<Rigidbody>();
    }
    
    public void setCarried(bool carried) => isCarried = carried;
    public bool GetCarriedStatus() => isCarried;
    public void setStored(bool stored) => isStored = stored;
    public bool GetStoredStatus() => isStored;

    public Vector3 GetOffset() => localOffset;
    public Vector3 GetRotation() => rotation;
    public InteractableObjectType GetCurrentType() => type;

    public void TempItemModeLogic(Transform point)
    {
        Debug.Log(isStored);
        if (isStored) PickupDelay(point);
        else StartCoroutine(PickupRoutine(point));
    }

    public void ItemLogic(Transform point = null, Interaction _interaction = null)
    {
        if (tempItemModel) Destroy(tempItemModel);
        if (_interaction) interaction = _interaction;
        if (type == InteractableObjectType.Item && isCarried) TempItemModeLogic(point);
        else if(!isCarried)
        {
            StopAllCoroutines();
            if (rigidBodyModel) rigidBodyModel.enabled = true;
            if (TryGetComponent(out Collider collider)) collider.enabled = true;
        }
    }

    void PickupDelay(Transform point)
    {
        tempItemModel = Instantiate(itemModel, point);
        rigidBodyModel.enabled = false;
    }

    private IEnumerator PickupRoutine(Transform point)
    {
        yield return new WaitForSeconds(pickupDelay);

        PickupDelay(point);
    }
}
