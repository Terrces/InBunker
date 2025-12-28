using UnityEngine;

public class Object : MonoBehaviour, Iinteractable, IdropableObject
{
    public Iinteractable.GameObjectTypes objectType { get; set; }

    [SerializeField] private Vector3 localOffset = new Vector3(0,-0.2f,2);
    [SerializeField] public Vector3 rotation;
    [SerializeField] private float SmoothTime = 0.3f;
    private LayerMask excludeLayer;
    Vector3 velocity = Vector3.one;
    private Transform hand;
    private Rigidbody rigidBody => GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        if (hand)
        {   
            rigidBody.rotation = Quaternion.Slerp(rigidBody.rotation, hand.rotation * Quaternion.Euler(rotation), SmoothTime);
            Vector3 target = hand.TransformPoint(localOffset);
            // Vector3 smothPosition = Vector3.Lerp(transform.position, target, moveSmoothTime);
            Vector3 smothPosition = Vector3.SmoothDamp(transform.position,target,ref velocity,SmoothTime);
            rigidBody.MovePosition(smothPosition);
        }
    }

    public void Interact(Transform _hand, LayerMask _layerMask)
    {
        hand = _hand;
        
        rigidBody.useGravity = false;
        excludeLayer = _layerMask;
        rigidBody.angularDamping = 0f;
        rigidBody.excludeLayers += excludeLayer;
    }
    
    public void OnDrop(float force = 0f)
    {
        transform.parent = null;
        Vector3 vec = hand.forward;
        rigidBody.excludeLayers -= excludeLayer;
        hand = null;
        rigidBody.useGravity = true;
        rigidBody.AddForce(vec * force,ForceMode.Impulse);
    }
}
