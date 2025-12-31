using UnityEngine;

public class Object : MonoBehaviour, Iinteractable, IdropableObject
{
    public Iinteractable.GameObjectTypes objectType { get; set; }

    [SerializeField] private Vector3 localOffset = new Vector3(0,-0.2f,2);
    [SerializeField] public Vector3 rotation;
    [SerializeField] private float SmoothTime = 0.3f;
    private LayerMask excludeLayer;
    private Transform hand;
    private Interaction interaction;
    private Rigidbody rigidBody => GetComponent<Rigidbody>();

    void Update()
    {
        if (hand)
        {   
            Vector3 target = hand.TransformPoint(localOffset);
            rigidBody.rotation = hand.rotation * Quaternion.Euler(rotation);
            rigidBody.linearVelocity = (target - transform.position) * SmoothTime;
        }
    }

    public void Interact(Interaction _interaction,Transform _hand, LayerMask _layerMask)
    {
        hand = _hand;
        interaction = _interaction;
        
        rigidBody.interpolation = RigidbodyInterpolation.None;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rigidBody.useGravity = false;
        excludeLayer = _layerMask;
        rigidBody.freezeRotation = true;
        rigidBody.excludeLayers += excludeLayer;
    }
    
    public void OnDrop(float force = 0f)
    {
        Vector3 vec = hand.forward;
        rigidBody.excludeLayers -= excludeLayer;
        interaction.carriedObject = null;
        hand = null;
        rigidBody.useGravity = true;
        rigidBody.freezeRotation = false;
        rigidBody.AddForce(vec * force,ForceMode.Impulse);
    }
}
