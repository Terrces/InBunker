using UnityEngine;

[SelectionBase]
public class Chunk : MonoBehaviour
{
    enum states {Generated,NextChunkGenerated,Unloaded}
    private int id;
    private GameObject location;
    private ChunkManager chunkManager;
    private states currentState;

    public void Init(int _id, Location _location)
    {
        id = _id;
        location = _location.LocationObject;

        Instantiate(location, position:transform.position, transform.rotation,parent:transform);
        chunkManager.chunkQueue.Add(this);
    }

    public void setChunkManager(ChunkManager _manager) => chunkManager = _manager;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!chunkManager) return;
        if (currentState == states.Generated) currentState = states.NextChunkGenerated;
        chunkManager.setPlayerChunk(id);

        if (currentState == states.Generated) chunkManager.GenerateNextChunk(false,chunkManager.getLocation(0));
    }
}
