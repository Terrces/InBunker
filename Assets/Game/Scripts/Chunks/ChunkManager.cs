using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class ChunkManager : MonoBehaviour
{
    [SerializeField] GameObject chunk;
    [SerializeField] List<GameObject> locations = new List<GameObject>();
    [SerializeField] Vector3 firstChunkPostion = new Vector3(0f,0f,0f);

    private Vector3 lastPosition;

    // Queue<GameObject> chunkQueue;

    void Start() => GenerateNextChunk(true);

    public GameObject getLocation(int _index)
    {
        return locations[_index];
    }

    public void GenerateNextChunk(bool firstChunk = false, GameObject Location = null)
    {
        GameObject _chunk = Instantiate(chunk,transform.position,transform.rotation,parent:transform);
        Chunk chunkComponent = _chunk.GetComponent<Chunk>();
        
        chunkComponent.setChunkManager(this);
        if (firstChunk) 
        {
            chunkComponent.setLocation(locations[0]);
            _chunk.transform.position = firstChunkPostion;
        }
        chunkComponent.Init();

        if (firstChunk) return;
        
        lastPosition += chunkComponent.GetOffset();
        _chunk.transform.position += lastPosition;
    }
    private void unloadLastChunk()
    {
        
    }
}
