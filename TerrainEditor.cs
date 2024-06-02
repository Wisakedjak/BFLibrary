namespace BFL;
{
    using System;
using UnityEngine;
 
/// <summary>
/// A tool for modifying terrain.
/// </summary>
public sealed class TerrainEditor : MonoBehaviour
{
    /// <summary>
    /// The width of the brush.
    /// </summary>
    [SerializeField]
    [Tooltip("The width of the brush.")]
    private int brushWidth;
 
    /// <summary>
    /// The height of the brush.
    /// </summary>
    [SerializeField]
    [Tooltip("The height of the brush.")]
    private int brushHeight;
 
    /// <summary>
    /// The strength of the brush.
    /// </summary>
    [SerializeField]
    [Tooltip("The strength of the brush.")]
    private float strength = 0.05f;
 
    /// <summary>
    /// The action to perform when modifying the terrain.
    /// </summary>
    [SerializeField]
    [Tooltip("The action to perform when modifying the terrain.")]
    private TerrainModificationAction modificationAction;
 
    /// <summary>
    /// The camera used for ray casting.
    /// </summary>
    private Camera _camera;
 
    /// <summary>
    /// The terrain to modify.
    /// </summary>
    private Terrain _targetTerrain;
 
    /// <summary>
    /// The terrain data of the terrain to modify.
    /// </summary>
    private TerrainData _targetTerrainData;
 
    /// <summary>
    /// The height sampled from the terrain.
    /// </summary>
    private float _sampledHeight;
 
    /// <summary>
    /// The actions that can be performed when modifying the terrain.
    /// </summary>
    private enum TerrainModificationAction
    {
        Raise,
        Lower,
        Flatten,
        Sample,
        SampleAverage,
        Smooth,
    }
 
    private void Start()
    {
        _camera = Camera.main;
    }
 
    private void Update()
    {
        if (!Input.GetMouseButton(0) || !Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo)) return;
 
        if (hitInfo.transform.TryGetComponent(out Terrain terrain))
        {
            _targetTerrain = terrain;
 
            _targetTerrainData = _targetTerrain.terrainData;
        }
        else
        {
            return;
        }
 
        switch (modificationAction)
        {
            case TerrainModificationAction.Raise:
            {
                RaiseTerrain(hitInfo.point);
 
                break;
            }
 
            case TerrainModificationAction.Lower:
            {
                LowerTerrain(hitInfo.point);
 
                break;
            }
 
            case TerrainModificationAction.Flatten:
            {
                FlattenTerrain(hitInfo.point, _sampledHeight);
 
                break;
            }
 
            case TerrainModificationAction.Sample:
            {
                _sampledHeight = SampleHeight(hitInfo.point);
 
                break;
            }
 
            case TerrainModificationAction.SampleAverage:
            {
                _sampledHeight = SampleAverageHeight(hitInfo.point);
 
                break;
            }
 
            case TerrainModificationAction.Smooth:
            {
                SmoothTerrain(hitInfo.point);
 
                break;
            }
 
            default:
            {
                Debug.LogError("Invalid terrain modification action.", this);
 
                break;
            }
        }
    }
 
    private Vector3 WorldToTerrainPosition(Vector3 worldPosition)
    {
        Vector3 terrainPosition = worldPosition - _targetTerrain.GetPosition();
 
        Vector3 terrainSize = _targetTerrainData.size;
 
        int heightmapResolution = _targetTerrainData.heightmapResolution;
 
        terrainPosition = new Vector3(terrainPosition.x / terrainSize.x, terrainPosition.y / terrainSize.y, terrainPosition.z / terrainSize.z);
 
        return new Vector3(terrainPosition.x * heightmapResolution, 0.0f, terrainPosition.z * heightmapResolution);
    }
 
    private (int, int) ClampBrushPosition(Vector3 brushWorldPosition)
    {
        Vector3 terrainPosition = WorldToTerrainPosition(brushWorldPosition);
 
        int heightmapResolution = _targetTerrainData.heightmapResolution;
 
        int clampedBrushX = (int)Math.Min(Math.Max((terrainPosition.x - brushWidth * 0.5f), 0), heightmapResolution);
        int clampedBrushY = (int)Math.Min(Math.Max((terrainPosition.z - brushHeight * 0.5f), 0), heightmapResolution);
 
        return (clampedBrushX, clampedBrushY);
    }
 
    private (int, int) ClampBrushSize(int brushX, int brushY)
    {
        int heightmapResolution = _targetTerrainData.heightmapResolution;
 
        int clampedBrushWidth = Math.Min(brushWidth, heightmapResolution - brushX);
        int clampedBrushHeight = Math.Min(brushHeight, heightmapResolution - brushY);
 
        return (clampedBrushWidth, clampedBrushHeight);
    }
 
    private void RaiseTerrain(Vector3 brushWorldPosition)
    {
        (int clampedBrushX, int clampedBrushY) = ClampBrushPosition(brushWorldPosition);
 
        (int clampedBrushWidth, int clampedBrushHeight) = ClampBrushSize(clampedBrushX, clampedBrushY);
 
        float[,] heights = _targetTerrainData.GetHeights(clampedBrushX, clampedBrushY, clampedBrushWidth, clampedBrushHeight);
 
        float increment = strength * Time.deltaTime;
 
        for (int y = 0; y < clampedBrushHeight; y++)
        {
            for (int x = 0; x < clampedBrushWidth; x++)
            {
                heights[y, x] += increment;
            }
        }
 
        _targetTerrainData.SetHeights(clampedBrushX, clampedBrushY, heights);
    }
 
    private void LowerTerrain(Vector3 brushWorldPosition)
    {
        (int clampedBrushX, int clampedBrushY) = ClampBrushPosition(brushWorldPosition);
 
        (int clampedBrushWidth, int clampedBrushHeight) = ClampBrushSize(clampedBrushX, clampedBrushY);
 
        float[,] heights = _targetTerrainData.GetHeights(clampedBrushX, clampedBrushY, clampedBrushWidth, clampedBrushHeight);
 
        float decrement = strength * Time.deltaTime;
 
        for (int y = 0; y < clampedBrushHeight; y++)
        {
            for (int x = 0; x < clampedBrushWidth; x++)
            {
                heights[y, x] -= decrement;
            }
        }
 
        _targetTerrainData.SetHeights(clampedBrushX, clampedBrushY, heights);
    }
 
    private void FlattenTerrain(Vector3 brushWorldPosition, float height)
    {
        (int clampedBrushX, int clampedBrushY) = ClampBrushPosition(brushWorldPosition);
 
        (int clampedBrushWidth, int clampedBrushHeight) = ClampBrushSize(clampedBrushX, clampedBrushY);
 
        float[,] heights = _targetTerrainData.GetHeights(clampedBrushX, clampedBrushY, clampedBrushWidth, clampedBrushHeight);
 
        for (int y = 0; y < clampedBrushHeight; y++)
        {
            for (int x = 0; x < clampedBrushWidth; x++)
            {
                heights[y, x] = height;
            }
        }
 
        _targetTerrainData.SetHeights(clampedBrushX, clampedBrushY, heights);
    }
 
    private float SampleHeight(Vector3 worldPosition)
    {
        Vector3 terrainPosition = WorldToTerrainPosition(worldPosition);
 
        return _targetTerrainData.GetInterpolatedHeight((int)terrainPosition.x, (int)terrainPosition.z);
    }
 
    private float SampleAverageHeight(Vector3 brushWorldPosition)
    {
        (int clampedBrushX, int clampedBrushY) = ClampBrushPosition(brushWorldPosition);
 
        (int clampedBrushWidth, int clampedBrushHeight) = ClampBrushSize(clampedBrushX, clampedBrushY);
 
        float[,] heights = _targetTerrainData.GetHeights(clampedBrushX, clampedBrushY, clampedBrushWidth, clampedBrushHeight);
 
        float sum = 0.0f;
 
        for (int y = 0; y < clampedBrushHeight; y++)
        {
            for (int x = 0; x < clampedBrushWidth; x++)
            {
                sum += heights[y, x];
            }
        }
 
        return sum / (clampedBrushWidth * clampedBrushHeight);
    }
 
    private void SmoothTerrain(Vector3 brushWorldPosition)
    {
        (int clampedBrushX, int clampedBrushY) = ClampBrushPosition(brushWorldPosition);
 
        (int clampedBrushWidth, int clampedBrushHeight) = ClampBrushSize(clampedBrushX, clampedBrushY);
 
        float[,] heights = _targetTerrainData.GetHeights(clampedBrushX, clampedBrushY, clampedBrushWidth, clampedBrushHeight);
 
        float[,] smoothedHeights = new float[clampedBrushHeight, clampedBrushWidth];
 
        for (int y = 0; y < clampedBrushHeight; y++)
        {
            for (int x = 0; x < clampedBrushWidth; x++)
            {
                float sum = heights[y, x];
 
                int count = 1;
 
                if (x > 0)
                {
                    sum += heights[y, x - 1];
 
                    count++;
                }
 
                if (x < clampedBrushWidth - 1)
                {
                    sum += heights[y, x + 1];
 
                    count++;
                }
 
                if (y > 0)
                {
                    sum += heights[y - 1, x];
 
                    count++;
                }
 
                if (y < clampedBrushHeight - 1)
                {
                    sum += heights[y + 1, x];
 
                    count++;
                }
 
                smoothedHeights[y, x] = sum / count;
            }
        }
 
        _targetTerrainData.SetHeights(clampedBrushX, clampedBrushY, smoothedHeights);
    }
}
 
}