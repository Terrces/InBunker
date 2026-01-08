using UnityEngine;

public class Object : MonoBehaviour, Iinteractable, IdropableObject
{
    [SerializeField] private Vector3 localOffset = new Vector3(0,-0.2f,2);
    [SerializeField] public Vector3 rotation;
    private float moveSpeed;
    private LayerMask excludeLayer;
    private Transform Point;
    public Interaction interaction;
    private Properties Properties;
    private Rigidbody rigidBody => GetComponent<Rigidbody>();
    private float maxDistance;

    void FixedUpdate()
    {
        if (Point)
        {   
            Vector3 target = Point.TransformPoint(localOffset);
            Vector3 finalTarget = target - transform.position;
            
            if (finalTarget.sqrMagnitude > maxDistance)
            {
                Drop();
                return;
            }

            rigidBody.rotation = Point.rotation * Quaternion.Euler(rotation);
            rigidBody.linearVelocity = finalTarget * (Time.fixedDeltaTime * (moveSpeed * 100)/rigidBody.mass);
        }
    }

    public void Interact(Interaction _interaction, Transform _point, float _moveSpeed, float _maxDistance, LayerMask _layerMask, Properties _properties)
    {
        Point = _point;
        interaction = _interaction;
        moveSpeed = _moveSpeed;
        maxDistance = _maxDistance;
        Properties = _properties;
        
        transform.SetParent(null);

        rigidBody.linearVelocity = Vector3.zero;
        rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
        rigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rigidBody.useGravity = false;
        excludeLayer = _layerMask;
        rigidBody.freezeRotation = true;
        rigidBody.excludeLayers += excludeLayer;
    }
    
    public void Drop(float force = 0f)
    {
        Vector3 vec = Vector3.zero;
        if (Point != null) 
        {
            vec = Point.forward;
            Point = null;
        }
        rigidBody.excludeLayers -= excludeLayer;
        
        rigidBody.useGravity = true;
        rigidBody.freezeRotation = false;
        rigidBody.AddForce(vec * (force-rigidBody.mass),ForceMode.Impulse);

        if (interaction != null) interaction.dropObject();
        if (Properties != null) transform.SetParent(Properties.chunkManager.chunkQueue[Properties.chunkManager.GetPlayerCurrentChunkID()-1].transform);
    }
}
