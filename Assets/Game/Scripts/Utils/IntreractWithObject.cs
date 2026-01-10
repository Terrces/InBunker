using UnityEngine;

public class InteractWithObject
{


    /// <summary>
    /// 
    /// </summary>
    /// <param name="point"></param>
    /// <param name="properties"></param>
    /// <returns> returns raycast hit collider </returns>
    public Collider GetRaycastHitCollider(Transform point, Properties properties, LayerMask mask)
    {
        Ray ray = new Ray(point.position,point.forward);
        if (Physics.Raycast(ray, out RaycastHit hit,properties.GetInteractDistance(), mask) && hit.collider != null) return hit.collider;
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="point"></param>
    /// <param name="properties"></param>
    /// <returns> returns raycast hit collider </returns>
    public Collider GetRaycastHitCollider(Transform point, Properties properties)
    {
        Ray ray = new Ray(point.position,point.forward);
        if (Physics.Raycast(ray, out RaycastHit hit,properties.GetInteractDistance(), 1, QueryTriggerInteraction.Ignore) && hit.collider != null) return hit.collider;
        return null;
    }
    public Collider GetColliderAndHitObject(Transform point, Properties properties, float impulse)
    {
        Ray ray = new Ray(point.position,point.forward);
        if (Physics.Raycast(ray, out RaycastHit hit,properties.GetInteractDistance()) && hit.collider != null)
        {
            if(hit.collider.TryGetComponent(out Rigidbody rigidbody))
            {
                Vector3 forceDir = (-hit.normal + point.forward).normalized;
                rigidbody.AddForceAtPosition(forceDir * impulse, hit.point, ForceMode.Impulse);
            }
            return hit.collider;
        }
        return null;
    }
}
