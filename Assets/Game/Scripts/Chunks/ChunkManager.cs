using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[SelectionBase]
public class ChunkManager : MonoBehaviour
{
    [SerializeField] GameObject chunk;
    [SerializeField] List<GameObject> locations = new List<GameObject>();
    [SerializeField] Vector3 firstChunkPostion = new Vector3(0f,0f,0f);
    [SerializeField] Queue<GameObject> chunksQueue;

    private int playerInChunkId = 0;
    private int lastChunkID = 0;

    private Vector3 lastPosition;
    private Vector3 lastRotation;

    // Queue<GameObject> chunkQueue;

    void Start() => GenerateNextChunk(true,locations[0]);

    public void setPlayerChunk(int _id) => playerInChunkId = _id;
    public GameObject getLocation(int _index) => locations[_index];

    public void GenerateNextChunk(bool firstChunk = false, GameObject _location = null)
    {
        GameObject _chunk = Instantiate(chunk,transform.position,transform.rotation,parent:transform);
        Chunk chunkComponent = _chunk.GetComponent<Chunk>();
        
        chunkComponent.setChunkManager(this);

        int chunkID = lastChunkID += 1; 

        if (firstChunk) 
        {
            _chunk.transform.position = firstChunkPostion;
            lastPosition = firstChunkPostion;
            chunkComponent.Init(0,_location);
            lastChunkID = 0;
            return;
        }

        chunkComponent.Init(chunkID, _location);
        lastPosition += chunkComponent.GetOffset();
        if (chunkComponent.GetLocationRotating()) _chunk.transform.rotation = addChunkRotation(chunkComponent.GetRotationRadius());
        // Debug.Log(lastPosition);
        _chunk.transform.position += lastPosition;
    }
    
    private quaternion addChunkRotation(Vector3 _rotation)
    {
        quaternion rotation = quaternion.Euler(
            math.radians(lastRotation.x += _rotation.x), 
            math.radians(lastRotation.y += _rotation.y), 
            math.radians(lastRotation.z += _rotation.z)
            );
        Debug.Log(rotation);
        return rotation;
    }

    private void unloadLastChunk()
    {
        
    }
}
