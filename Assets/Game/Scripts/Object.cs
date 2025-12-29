using UnityEngine;

public class Object : MonoBehaviour, Iinteractable, IdropableObject
{
    public Iinteractable.GameObjectTypes objectType { get; set; }

    [SerializeField] private Vector3 localOffset = new Vector3(0,-0.2f,2);
    [SerializeField] public Vector3 rotation;
    [SerializeField] private float SmoothTime = 0.3f;
    private LayerMask excludeLayer;
    private Transform hand;
    private Rigidbody rigidBody => GetComponent<Rigidbody>();

    void FixedUpdate()
    {
        if (hand)
        {   
            Vector3 target = hand.TransformPoint(localOffset);
            rigidBody.rotation = hand.rotation * Quaternion.Euler(rotation);
            Vector3 smoothPosition = Vector3.Lerp(transform.position, target, Time.fixedDeltaTime * SmoothTime);
            rigidBody.position = smoothPosition;
        }
    }

    public void Interact(Transform _hand, LayerMask _layerMask)
    {
        hand = _hand;
        
        rigidBody.interpolation = RigidbodyInterpolation.None;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
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
