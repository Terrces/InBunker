using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class ChunkManager : MonoBehaviour
{
    public enum GameBiomes {Spawn, StandartBiome}
    [SerializeField] GameObject chunk;
    [SerializeField] List<UpdateBeforeChunkID> UpdatesBiomes;
    [SerializeField] List<Location> locations = new List<Location>();
    [SerializeField] Vector3 firstChunkPostion = new Vector3(0f,0f,0f);
    [SerializeField] int preLoadChunks = 4;
    [SerializeField] Queue<GameObject> chunksQueue;
    [SerializeField] Properties playerProperties;
    public GameBiomes CurrentBiome = GameBiomes.Spawn;
    private int lastChunkID = 0;

    private Vector3 lastPosition;
    private Vector3 lastRotation;

    public List<GameObject> chunkQueue;

    void Start() => PreGenerateChunks();
    private void PreGenerateChunks()
    {
        for(int i = 0; i <= preLoadChunks; i++) Generate();
    }
    public void setPlayerChunk(int _id) => playerProperties.UpdateChunkID(_id);
    public Location getLocation(int _index) => locations[_index];
    
    [ContextMenu("Reset Generation")]
    public void ResetGeneraton()
    {
        foreach (GameObject obj in chunkQueue)
        {
            Destroy(obj);
        }
        lastChunkID = 0;
        lastPosition = Vector3.zero;
        lastRotation = Vector3.zero;
        chunkQueue.Clear();

        PreGenerateChunks();
    }

    private void firstChunkGenerate(int Value)
    {
        if(lastChunkID != 0) return;

        for (int i = 0; i <= locations.Count; i++)
        {
            if(UpdatesBiomes[Value].Biome == GameBiomes.Spawn) UpdatesBiomes[Value].Enable = false;
            if (locations[i].Biome == GameBiomes.Spawn)
            {
                GenerateNextChunk(true, locations[i]);
                return;
            }
            if(i == locations.Count-1)
            {
                GenerateNextChunk(true, locations[0]);
                return;
            }
        }
    }

    public void Generate()
    {
        for (int i = 0; i < UpdatesBiomes.Count; i++)
        {
            if (UpdatesBiomes[i].Enable)
            {
                if(lastChunkID == 0) firstChunkGenerate(i);
                else
                {
                    for (int j = 0; j < locations.Count; j++)
                    {
                        if(UpdatesBiomes[i].Biome == locations[j].Biome)
                        {
                            if(locations[j].SpawnEveryXChunk != 0 && locations[j].SpawnEveryXChunk % lastChunkID != 0) return;
                            GenerateNextChunk(false,locations[j]);
                        }
                    }
                }
            }
        }
    }

    public void GenerateNextChunk(bool firstChunk = false, Location _location = null)
    {
        int chunkID = lastChunkID += 1; 

        quaternion rotation;
        Vector3 position = Vector3.zero;

        if (_location.EnableRotating) rotation = addChunkRotation(_location.TurnRadian);
        else  rotation = quaternion.Euler(Vector3.zero);
        if(_location.TurnRadianNextChunk != Vector3.zero)
        {
            lastRotation.x += _location.TurnRadianNextChunk.x;
            lastRotation.y += _location.TurnRadianNextChunk.y;
            lastRotation.z += _location.TurnRadianNextChunk.z;
        }

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
        chunkComponent.Init(chunkID, _location, this);
        chunkQueue.Add(_chunk);
    }

    public void loadChunk()
    {
        List<GameObject> _chunkQueue = chunkQueue.AsEnumerable().Reverse().ToList();
        foreach (GameObject chunk in _chunkQueue)
        {
            if (!chunk.activeInHierarchy)
            {
                chunk.SetActive(true);
                return;
            } 
        }
    }

    public void unloadLastChunk() => SetChunkActive(false, 5);


    private void SetChunkActive(bool active, int id = 3)
    {
        int minPlayerId = id-1;
        if(chunkQueue.Count >= preLoadChunks && minPlayerId < playerProperties.CurrentChunkID )
        {
            int ID = playerProperties.CurrentChunkID - id;
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
