using UnityEngine;

[SelectionBase]
public class ChunkLocation : MonoBehaviour
{
    [SerializeField] Vector3 offset;

    public Vector3 GetOffset()
    {
        return offset;
    }
}
