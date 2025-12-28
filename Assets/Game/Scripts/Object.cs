using DG.Tweening;
using UnityEngine;

public class Object : MonoBehaviour, Iinteractable, IdropableObject
{
    public Iinteractable.GameObjectTypes objectType { get; set; }

    [SerializeField] private Vector3 localOffset;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float moveSmoothTime = 0.15f;

    private Rigidbody rb => GetComponent<Rigidbody>();
    private Collider col => GetComponent<Collider>();

    private bool tempFlag = false;

    Transform arm;

    public void Interact(Interaction interaction)
    {
        arm = interaction.GetArm().transform;

        rb.isKinematic = true;
        rb.useGravity = false;
        col.enabled = false;

        transform.SetParent(arm);
        transform.localRotation = Quaternion.Euler(rotation);
    }

    public void SetTargetPosition(Vector3 worldPosition, bool isAtArm)
    {
        if (!isAtArm) 
        {
            tempFlag = false;
            
            // Ключевой момент: переводим мировую точку в локальную для родителя (рук)
            Vector3 localPos = transform.parent.InverseTransformPoint(worldPosition);
            Vector3 finalPos = new Vector3(
                localPos.normalized.x + localOffset.x, 
                localPos.normalized.y + localOffset.y, 
                localPos.z
            );
            // Debug.Log(finalPos.z);
            transform.DOLocalMove(finalPos, moveSmoothTime);
        }
        else if (!tempFlag)
        {
            // Здесь используем заранее заданный локальный офсет (например, 0,0,0)
            transform.DOLocalMove(localOffset, moveSmoothTime);
            tempFlag = true;
        }
    }

    public void OnDrop(float force)
    {
        tempFlag = false;
        transform.DOKill();
        transform.SetParent(null);

        col.enabled = true;
        rb.isKinematic = false;
        rb.useGravity = true;

        rb.AddForce(arm.forward * force, ForceMode.Impulse);
    }
}
