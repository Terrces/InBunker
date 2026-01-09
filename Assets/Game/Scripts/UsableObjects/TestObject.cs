using UnityEngine;

public class TestObject : MonoBehaviour, Iusable
{
    public void Use(Transform camera, Properties properties)
    {
        InteractWithObject interactWithObject = new InteractWithObject();

        if(interactWithObject.GetRaycastHit(camera,properties).collider.TryGetComponent(out DestroyableObject component)) component.GetDamage();
    }
}
