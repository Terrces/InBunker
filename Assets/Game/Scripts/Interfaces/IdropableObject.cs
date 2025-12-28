using UnityEngine;
public interface IdropableObject
{
    public void OnDrop(float force = 0f);
    public void SetTargetPosition(Vector3 worldPosition, bool arm);
}
