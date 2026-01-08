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

    public void Init(int _id, Location _location, ChunkManager _manager)
    {
        id = _id;
        location = _location.LocationObject;
        chunkManager = _manager;
        Instantiate(location, position:transform.position, transform.rotation,parent:transform);
    }
    public int GetChunkID() => id;
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!chunkManager) return;
        chunkManager.setPlayerChunk(id);
        if (currentGenerationState == chunkGenerationStates.Generated) 
        {
            chunkManager.Generate();
            currentGenerationState = chunkGenerationStates.NextChunkGenerated;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
    }
}
