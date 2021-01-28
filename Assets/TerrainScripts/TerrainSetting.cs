using System;
using System.Numerics;
using UnityEngine;

[Serializable]
public class TerrainSetting
{

    public SizeSetting Size;
    public Vector3Int chunkSize = Vector3Int.one * 32;
    public Vector2Int chunkSpawnSize = Vector2Int.one * 3;
}

[Serializable]
public enum SizeSetting
{

    Small,
    Medium,
    Large,
    Random,
}