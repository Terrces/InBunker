using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[SelectionBase]
public class ChunkManager : MonoBehaviour
{
    [SerializeField] GameObject chunk;
    [SerializeField] List<GameObject> locations = new List<GameObject>();
    [SerializeField] Vector3 firstChunkPostion = new Vector3(0f,0f,0f);
    [SerializeField] Queue<GameObject> chunksQueue;

    private int playerInChunkId = 0;

    private Vector3 lastPosition;

    // Queue<GameObject> chunkQueue;

    void Start() => GenerateNextChunk(true);

    public void setPlayerChunk(int _id) {playerInChunkId = _id; Debug.Log(_id);} 

    public GameObject getLocation(int _index)
    {
        return locations[_index];
    }

    public void GenerateNextChunk(bool firstChunk = false, GameObject _location = null)
    {
        GameObject _chunk = Instantiate(chunk,transform.position,transform.rotation,parent:transform);
        Chunk chunkComponent = _chunk.GetComponent<Chunk>();
        
        chunkComponent.setChunkManager(this);

        if (firstChunk) 
        {
            _chunk.transform.position = firstChunkPostion;
            chunkComponent.Init(0,locations[0]);
            return;
        }

        chunkComponent.Init(playerInChunkId + 1, _location);
        lastPosition += chunkComponent.GetOffset();
        Debug.Log(lastPosition);
        _chunk.transform.position += lastPosition;
    }
    
    private void unloadLastChunk()
    {
        
    }
}
