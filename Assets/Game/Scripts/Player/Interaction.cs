using UnityEngine;

public class Interaction:MonoBehaviour
{
    [SerializeField] private GameObject armPoint;
    [SerializeField] private Vector3 defaultArmPosition;
    [SerializeField] private float distance = 5f;
    public void OnInteract()
    {
        Player player = GetComponent<Player>();
        RaycastHit hit;
        if (Physics.Raycast(player.cameraTransform.position,player.cameraTransform.transform.TransformDirection(Vector3.forward),out hit, distance))
        {
            Debug.Log(hit.collider.gameObject.name);
            
        }
    }
}
