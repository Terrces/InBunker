using UnityEngine;

[SelectionBase]
public class Chunk : MonoBehaviour
{
    enum states {Generated,NextChunkGenerated,Unloaded}
    private int id;
    private GameObject location;
    private ChunkManager chunkManager;
    private Vector3 offset;
    private bool enableLocationRotating;
    private Vector3 rotationRadius;
    private states currentState;

    public void Init(int _id, GameObject _location)
    {
        id = _id;
        // Debug.Log(id);
        location = _location;

        GameObject _locationObject = Instantiate(_location, position:transform.position, transform.rotation,parent:transform);
        ChunkLocation chunkLocation = _locationObject.GetComponent<ChunkLocation>();

        offset = chunkLocation.GetOffset();
        enableLocationRotating = chunkLocation.GetLocationRotating();
        rotationRadius = chunkLocation.GetRotationRadius();
    }

    public void setChunkManager(ChunkManager _manager) => chunkManager = _manager;
    public void setLocation(GameObject _location) => location = _location;
    public Vector3 GetOffset() => offset;
    public bool GetLocationRotating() => enableLocationRotating;
    public Vector3 GetRotationRadius() => rotationRadius;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!chunkManager) return;
        if (currentState == states.Generated) chunkManager.GenerateNextChunk(false,chunkManager.getLocation(0));
        chunkManager.setPlayerChunk(id);

        if (currentState == states.Generated) currentState = states.NextChunkGenerated;

        // Debug.Log("Add unload last chunks");
    }
}
