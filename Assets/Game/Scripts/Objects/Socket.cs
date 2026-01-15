using DG.Tweening;
using UnityEngine;

public class Socket : MonoBehaviour
{
    [SerializeField] Transform socket;
    public void GetSocket(Collider collider)
    {
        if (socket)
        {            
            if(collider.TryGetComponent(out InteractiveObject interactiveObject) && interactiveObject.GetCarriedStatus()) interactiveObject.interaction.DropObject();
            collider.GetComponent<Rigidbody>().isKinematic = true;
            collider.transform.DOMove(socket.position,0.2f);
            collider.transform.rotation = Quaternion.Euler(collider.GetComponent<InteractiveObject>().GetRotation());
            collider.transform.position = socket.position;
        }
    }
}
