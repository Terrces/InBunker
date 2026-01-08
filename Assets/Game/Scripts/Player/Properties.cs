using UnityEngine;

public class Properties : MonoBehaviour
{
    public int CurrentChunkID = 1;
    public int PreviouslyChunkID = 1;

    public ChunkManager chunkManager;

    public void UpdateChunkID(int ChunkID)
    {
        if (PreviouslyChunkID != CurrentChunkID) PreviouslyChunkID = CurrentChunkID;
        if (CurrentChunkID != ChunkID) CurrentChunkID = ChunkID;
        if (PreviouslyChunkID > CurrentChunkID) chunkManager.loadChunk();
        else chunkManager.unloadLastChunk();
    }
}
