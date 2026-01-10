using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public Storage storage;
    // [SerializeField] private float maxForceSpeed = 5.5f;
    [SerializeField] private bool CollisionDamage;
    [SerializeField] private float durability = 2;
    private Rigidbody rigidBody => GetComponent<Rigidbody>();
    // private Object carriedObject;
    private bool isQuitting = false;
    private float impact = 1;
    private GameObject item;

    void Start()
    {
        if (TryGetComponent(out Storage _storage)) storage = _storage;
        // if (TryGetComponent(out Object _object)) carriedObject = _object;
        // if (storage != null) item = storage.GetItem();
    }
    
    void OnCollisionEnter(Collision collision) => collisionDamage(collision);

    private void collisionDamage(Collision collision)
    {
        collision.collider.TryGetComponent(out Rigidbody _rigidBody);
        
        if (!CollisionDamage) return;
        if (_rigidBody != null) return;

        if (collision.relativeVelocity.sqrMagnitude >= rigidBody.mass*rigidBody.mass)
        {
            impact = rigidBody.mass;
            GetDamage(impact);
        }
    }

    public void GetDamage(float _impact = 1)
    {
        impact = _impact;
        damage();
        @break();
        impact = _impact;
    }

    private bool damage()
    {
        durability -= impact;
        impact = 0;
        return durability > 0;
    }

    private void @break()
    {
        if (isQuitting) return;
        if (damage() == true) return;

        isQuitting = true;

        // if I'm wanna comeback to this logic
        // if (carriedObject != null && carriedObject.Point != null) carriedObject.Drop();
        if (storage != null) item = storage.GetItem();
        if (item != null)
        {
            GameObject _gameObject = Instantiate(item,transform.position,transform.rotation,transform.parent);
            if( _gameObject.TryGetComponent(out Rigidbody rigidbodyComponent)) rigidbodyComponent.linearVelocity = rigidBody.linearVelocity;
        }
        
        Destroy(gameObject);
    }
}
