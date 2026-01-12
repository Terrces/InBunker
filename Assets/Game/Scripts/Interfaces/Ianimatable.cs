using UnityEngine;

public struct AnimationContext
{
    Transform transform;
    Vector3 vector3;
}

public interface Ianimatable
{
    public void Animate(AnimationContext context);
}
