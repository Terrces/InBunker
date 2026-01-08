using System;
using UnityEngine;

[Serializable]
public class Location
{
    public GameObject LocationObject;
    public Vector3 Offset;
    public Vector3 TurnRadian;
    public Vector3 TurnRadianNextChunk;
    public ChunkManager.GameBiomes Biome = ChunkManager.GameBiomes.StandartBiome;
    public int SpawnEveryXChunk;
    public bool EnableRotating;
}
