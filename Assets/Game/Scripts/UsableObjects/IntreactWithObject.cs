using UnityEngine;

public class InteractWithObject
{
    public RaycastHit GetRaycastHit(Transform point, Properties properties)
    {
        Ray ray = new Ray(point.position,point.forward);
        if (Physics.Raycast(ray, out RaycastHit hit,properties.GetInteractDistance(), 1, QueryTriggerInteraction.Ignore)) return hit;
        return new RaycastHit();
    }
}
