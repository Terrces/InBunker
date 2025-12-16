using UnityEngine;

[SelectionBase]
public class Chunk : MonoBehaviour
{
    private GameObject location;
    private ChunkManager chunkManager;

    private Vector3 offset;

    private bool firstChunk;

    public void Init(bool _first = false)
    {
        if (location == null) return;

        GameObject _location = Instantiate(location, position:transform.position, transform.rotation,parent:transform);
        ChunkLocation chunkLocation = _location.GetComponent<ChunkLocation>();
        offset = chunkLocation.GetOffset();
    }

    public void setChunkManager(ChunkManager _manager) => chunkManager = _manager;
    public void setLocation(GameObject _location) => location = _location;

    public Vector3 GetOffset() => offset;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Add generation next chunk");
        if (!chunkManager) return;
        chunkManager.GenerateNextChunk(chunkManager.getLocation(0));
        GetComponent<BoxCollider>().enabled = false;
    }
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Add unload last chunks");
    }
}
