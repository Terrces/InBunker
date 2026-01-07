using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[SelectionBase]
public class ChunkManager : MonoBehaviour
{
    [SerializeField] GameObject chunk;
    [SerializeField] List<Location> locations = new List<Location>();
    [SerializeField] Vector3 firstChunkPostion = new Vector3(0f,0f,0f);
    [SerializeField] Queue<GameObject> chunksQueue;
    [SerializeField] Properties playerProperties;

    private int lastChunkID = 0;

    private Vector3 lastPosition;
    private Vector3 lastRotation;

    public List<GameObject> chunkQueue;

    void Start()
    {
        GenerateNextChunk(true,locations[0]);
        
        for (int i = 0; chunkQueue.Count <= 4; i++)
        {
            GenerateNextChunk(false,locations[0]);
        }
    }

    public void setPlayerChunk(int _id, Chunk.chunkGenerationStates _generationState) => playerProperties.UpdateChunkID(_id, _generationState);
    public Location getLocation(int _index) => locations[_index];

    public void GenerateNextChunk(bool firstChunk = false, Location _location = null)
    {
        int chunkID = lastChunkID += 1; 

        quaternion rotation;
        Vector3 position = Vector3.zero;

        if (_location.EnableRotating && !firstChunk) rotation = addChunkRotation(_location.TurnRadian);
        else  rotation = quaternion.Euler(Vector3.zero);

        if (firstChunk)
        {
            lastPosition = firstChunkPostion;
            position = firstChunkPostion;
        }
        else
        {
            lastPosition += _location.Offset;
            position += lastPosition;
        }
        
        GameObject _chunk = Instantiate(chunk,position,rotation,parent:transform);
        Chunk chunkComponent = _chunk.GetComponent<Chunk>();
        chunkComponent.Init(chunkID, _location);
        chunkQueue.Add(_chunk);
        
        chunkComponent.setChunkManager(this);

        unloadLastChunk();
    }

    public void loadChunk() => SetChunkActive(true);

    private void unloadLastChunk() => SetChunkActive(false, 6);

    private void SetChunkActive(bool active, int id = 3, int minPlayerId = 5)
    {
        if(chunkQueue.Count >= 5 && minPlayerId < playerProperties.CurrentChunkID )
        {
            int ID = playerProperties.CurrentChunkID - id;
            Debug.Log(chunkQueue[ID].activeInHierarchy);
            if(chunkQueue[ID].activeInHierarchy != active)
            {
                chunkQueue[ID].SetActive(active);
            }
        }
    }
    
    private quaternion addChunkRotation(Vector3 _rotation)
    {
        quaternion rotation = quaternion.Euler(
            math.radians(lastRotation.x += _rotation.x), 
            math.radians(lastRotation.y += _rotation.y), 
            math.radians(lastRotation.z += _rotation.z)
            );
            
        return rotation;
    }

}
