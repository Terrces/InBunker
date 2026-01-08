using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

[SelectionBase]
public class ChunkManager : MonoBehaviour
{
    public enum GameBiomes {Spawn, StandartBiome}
    [SerializeField] GameObject chunk;
    [SerializeField] List<UpdateBeforeChunkID> UpdatesBiomes;
    [SerializeField] List<Location> locations = new List<Location>();
    [SerializeField] Vector3 firstChunkPostion = new Vector3(0f,0f,0f);
    [SerializeField] int preLoadChunks = 4;
    int PlayerCurrentChunkID = 1;
    int PlayerPreviouslyChunkID = 1;
    int lastQueueChunkID = 0;

    private Vector3 lastChunkPosition;
    private Vector3 lastChunkRotation;
    public List<GameObject> chunkQueue;

    void Start() => PreGenerateChunks();
    private void PreGenerateChunks()
    {
        for(int i = 0; i <= preLoadChunks; i++) Generate();
    }
    public void setPlayerChunk(int ChunkID)
    {
        if (PlayerPreviouslyChunkID != PlayerCurrentChunkID) PlayerPreviouslyChunkID = PlayerCurrentChunkID;
        if (PlayerCurrentChunkID != ChunkID) PlayerCurrentChunkID = ChunkID;
        if (PlayerPreviouslyChunkID > PlayerCurrentChunkID) loadChunk();
        else unloadLastChunk();
    }
    public int GetPlayerCurrentChunkID() => PlayerCurrentChunkID;
    public Location getLocation(int _index) => locations[_index];
    
    [ContextMenu("Reset Generation")]
    public void ResetGeneraton()
    {
        foreach (GameObject obj in chunkQueue) Destroy(obj);
        lastQueueChunkID = 0;
        lastChunkPosition = Vector3.zero;
        lastChunkRotation = Vector3.zero;
        chunkQueue.Clear();

        PreGenerateChunks();
    }

    private void firstChunkGenerate(int Value)
    {
        if(lastQueueChunkID != 0) return;
        if(UpdatesBiomes[Value].Biome == GameBiomes.Spawn) UpdatesBiomes[Value].Enable = false;
        setPlayerChunk(1);

        for (int i = 0; i <= locations.Count; i++)
        {
            if (locations[i].Biome == GameBiomes.Spawn)
            {
                GenerateChunk(true, locations[i]);
                return;
            }
            if(i == locations.Count-1)
            {
                GenerateChunk(true, locations[0]);
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
                if(lastQueueChunkID == 0) firstChunkGenerate(i);
                else
                {
                    for (int j = 0; j < locations.Count; j++)
                    {
                        if(UpdatesBiomes[i].Biome == locations[j].Biome)
                        {
                            if(locations[j].SpawnEveryXChunk != 0 && locations[j].SpawnEveryXChunk % lastQueueChunkID != 0) return;
                            GenerateChunk(false,locations[j]);
                        }
                    }
                }
            }
        }
    }

    private void GenerateChunk(bool firstChunk = false, Location _location = null)
    {
        int chunkID = lastQueueChunkID += 1; 

        quaternion rotation;
        Vector3 position = Vector3.zero;

        if (_location.EnableRotating) rotation = addChunkRotation(_location.TurnRadian);
        else  rotation = quaternion.Euler(Vector3.zero);
        if(_location.TurnRadianNextChunk != Vector3.zero)
        {
            lastChunkRotation.x += _location.TurnRadianNextChunk.x;
            lastChunkRotation.y += _location.TurnRadianNextChunk.y;
            lastChunkRotation.z += _location.TurnRadianNextChunk.z;
        }

        if (firstChunk)
        {
            lastChunkPosition = firstChunkPostion;
            position = firstChunkPostion;
        }
        else
        {
            lastChunkPosition += _location.Offset;
            position += lastChunkPosition;
        }
        
        GameObject _chunk = Instantiate(chunk,position,rotation,parent:transform);
        Chunk chunkComponent = _chunk.GetComponent<Chunk>();
        chunkComponent.Init(chunkID, _location, this);
        chunkQueue.Add(_chunk);
    }

    private void loadChunk()
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

    private void unloadLastChunk()
    {
        int id = 6;
        int minPlayerId = id-1;
        if(chunkQueue.Count >= preLoadChunks && minPlayerId < PlayerCurrentChunkID )
        {
            int ID = PlayerCurrentChunkID - id;
            chunkQueue[ID].SetActive(false);
        }
    }
    
    private quaternion addChunkRotation(Vector3 _rotation)
    {
        quaternion rotation = quaternion.Euler(
            math.radians(lastChunkRotation.x += _rotation.x), 
            math.radians(lastChunkRotation.y += _rotation.y), 
            math.radians(lastChunkRotation.z += _rotation.z)
            );
            
        return rotation;
    }

}
