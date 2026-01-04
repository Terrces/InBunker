using UnityEngine;

public class Properties : MonoBehaviour
{
    public int CurrentChunkID;
    public int PreviouslyChunkID;

    public ChunkManager chunkManager;
    // public Chunk.chunkGenerationStates currentChunkGenerationState;

    public void UpdateChunkID(int ChunkID,Chunk.chunkGenerationStates GenerationState)
    {
        if (PreviouslyChunkID != CurrentChunkID)
        {
            PreviouslyChunkID = CurrentChunkID;
            if (CurrentChunkID < PreviouslyChunkID)
            {
                chunkManager.loadChunk();
                Debug.Log("test 1");
            }
            Debug.Log($"Previously Chunk: {PreviouslyChunkID}");
        }
        if (CurrentChunkID != ChunkID)
        {
            CurrentChunkID = ChunkID;
            // currentChunkGenerationState = GenerationState;
            // Debug.Log($"Current chunk: {CurrentChunkID}");
        }
    }
}
