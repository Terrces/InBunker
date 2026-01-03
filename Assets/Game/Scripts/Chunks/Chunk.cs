using UnityEngine;

[SelectionBase]
public class Chunk : MonoBehaviour
{
    public enum chunkGenerationStates {Generated,NextChunkGenerated}
    public enum ChunkStates {Loaded,Unloaded}
    private int id;
    private GameObject location;
    private ChunkManager chunkManager;
    private chunkGenerationStates currentGenerationState;
    public ChunkStates currentChunkState = ChunkStates.Loaded;

    public void Init(int _id, Location _location)
    {
        id = _id;
        location = _location.LocationObject;

        Instantiate(location, position:transform.position, transform.rotation,parent:transform);
    }

    public void setChunkManager(ChunkManager _manager) => chunkManager = _manager;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!chunkManager) return;
        if (currentGenerationState == chunkGenerationStates.Generated) 
        {
            chunkManager.GenerateNextChunk(false,chunkManager.getLocation(0));
            currentGenerationState = chunkGenerationStates.NextChunkGenerated;
        }
        chunkManager.setPlayerChunk(id, currentGenerationState);
    }
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
    }
}
