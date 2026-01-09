using UnityEngine;

public class InteractWithObject
{
    public RaycastHit GetRaycastHit(Transform point, Properties properties)
    {
        Ray ray = new Ray(point.position,point.forward);
        if (Physics.Raycast(ray, out RaycastHit hit,properties.GetInteractDistance(), 1, QueryTriggerInteraction.Ignore)) return hit;
        return new RaycastHit();
    }
    
    public RaycastHit GetRaycastHit(Transform point, Properties properties, LayerMask mask)
    {
        Ray ray = new Ray(point.position,point.forward);
        if (Physics.Raycast(ray, out RaycastHit hit,properties.GetInteractDistance(), mask)) return hit;
        return new RaycastHit();
    }

}
