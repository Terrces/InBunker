using UnityEngine;

[SelectionBase]
public class Chunk : MonoBehaviour
{
    private int id;
    private GameObject location;
    private ChunkManager chunkManager;

    private Vector3 offset;

    public void Init(int _id, GameObject _location)
    {
        id = _id;
        location = _location;

        GameObject _locationObject = Instantiate(_location, position:transform.position, transform.rotation,parent:transform);
        ChunkLocation chunkLocation = _locationObject.GetComponent<ChunkLocation>();

        offset = chunkLocation.GetOffset();
    }

    public void setChunkManager(ChunkManager _manager) => chunkManager = _manager;
    public void setLocation(GameObject _location) => location = _location;
    public Vector3 GetOffset() => offset;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!chunkManager) return;
        chunkManager.GenerateNextChunk(false,chunkManager.getLocation(0));
        chunkManager.setPlayerChunk(id);
        
        GetComponent<BoxCollider>().enabled = false;
        Debug.Log("Add unload last chunks");
    }
}
