using System;

[Serializable]
public class UpdateBeforeChunkID
{
    public ChunkManager.GameBiomes Biome = ChunkManager.GameBiomes.StandartBiome;
    public bool Enable = true;
    public uint BeforeChunkID;
}
