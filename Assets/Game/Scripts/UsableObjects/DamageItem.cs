using UnityEngine;

public class DamageItem : MonoBehaviour, Iusable
{
    [SerializeField] private float impact = 10;
    [SerializeField] private float minDamage = 0f;
    [SerializeField] private float damage = 1;
    [SerializeField] private bool RandomDamage = false;
    public void Use(Transform camera, Properties properties)
    {
        Collider collider = new InteractWithObject().GetColliderAndHitObject(camera, properties, impact);
        if(collider != null && collider.TryGetComponent(out DestroyableObject component))
        {
            float _damage = damage;
            if (RandomDamage) _damage = Random.Range(minDamage,damage);
            component.GetDamage(_damage);
        }
    }
}
