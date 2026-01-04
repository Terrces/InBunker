using UnityEngine;

public class Object : MonoBehaviour, Iinteractable, IdropableObject
{
    [SerializeField] private Vector3 localOffset = new Vector3(0,-0.2f,2);
    [SerializeField] public Vector3 rotation;
    private float moveSpeed;
    private LayerMask excludeLayer;
    private Transform point;
    private Interaction interaction;
    private Rigidbody rigidBody => GetComponent<Rigidbody>();
    private float maxDistance;

    void FixedUpdate()
    {
        if (point)
        {   
            Vector3 target = point.TransformPoint(localOffset);
            Vector3 finalTarget = target - transform.position;
            
            if (finalTarget.sqrMagnitude > maxDistance)
            {
                Drop();
                return;
            }

            rigidBody.rotation = point.rotation * Quaternion.Euler(rotation);
            rigidBody.linearVelocity = finalTarget * (Time.fixedDeltaTime * (moveSpeed * 100));
        }
    }

    public void Interact(Interaction _interaction,Transform _point, float _moveSpeed, float _maxDistance, LayerMask _layerMask)
    {
        point = _point;
        interaction = _interaction;
        moveSpeed = _moveSpeed;
        maxDistance = _maxDistance;
        
        transform.SetParent(null);

        rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        rigidBody.useGravity = false;
        excludeLayer = _layerMask;
        rigidBody.freezeRotation = true;
        rigidBody.excludeLayers += excludeLayer;
    }
    
    public void Drop(float force = 0f, Properties properties = null)
    {
        Vector3 vec = point.forward;
        rigidBody.excludeLayers -= excludeLayer;
        
        rigidBody.useGravity = true;
        rigidBody.freezeRotation = false;
        rigidBody.AddForce(vec * force,ForceMode.Impulse);
        
        if (interaction.carriedObject != null) interaction.carriedObject = null;
        if (point != null) point = null;
        if (properties != null) transform.SetParent(properties.chunkManager.chunkQueue[properties.CurrentChunkID].transform);
    }
}
