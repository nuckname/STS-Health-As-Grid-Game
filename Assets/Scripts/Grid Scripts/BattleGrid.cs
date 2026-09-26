using System.Collections.Generic;
using UnityEngine;

public class BattleGrid : MonoBehaviour
{
    public static BattleGrid Instance;
    
    [Header("Grid Generation")]
    public GridCell cellPrefab;
    public int gridWidth = 5;
    public int gridHeight = 5;

    public float gridSizeWidth;
    public float gridSizeHeight;
    
    public float cellSpacing = 1.1f;

    [SerializeField] private Transform gridSpawnPosition;

    public Dictionary<Vector2Int, GridCell> gridCells = new Dictionary<Vector2Int, GridCell>();

    // State machine
    private void Awake()
    {
        Instance = this;
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        // Calculate offsets to center the grid on the gridSpawnPosition
        float offsetX = (gridWidth * cellSpacing) / 2f - (cellSpacing / 2f);
        float offsetY = (gridHeight * cellSpacing) / 2f - (cellSpacing / 2f);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                
                Transform parentTransform = gridSpawnPosition != null ? gridSpawnPosition : transform;
                GridCell newCell = Instantiate(cellPrefab, parentTransform);
                
                newCell.transform.localScale = new Vector3(gridSizeWidth, gridSizeHeight, 1f);
                
                newCell.transform.localPosition = new Vector3((x * cellSpacing) - offsetX, (y * cellSpacing) - offsetY, 0);
                newCell.gridPosition = pos;
                newCell.name = $"Cell {x}, {y}";

                gridCells[pos] = newCell;
            }
        }
    }

    public bool ValidateAndHighlightCells(Vector2Int centerPosition, List<Vector2> offsets)
    {
        ClearHighlights();
        
        List<Vector2Int> targetPositions = new List<Vector2Int>();
        
        foreach (Vector2 offset in offsets)
        {
            Vector2Int targetPos = centerPosition + new Vector2Int(Mathf.RoundToInt(offset.x), Mathf.RoundToInt(offset.y));
            
            if (!gridCells.ContainsKey(targetPos))
            {
                return false; 
            }
            targetPositions.Add(targetPos);
        }

        foreach (Vector2Int pos in targetPositions)
        {
            gridCells[pos].SetHighlight(true);
        }
        
        return true;
    }

    public void RemoveCells(Vector2Int centerPosition, List<Vector2> offsets)
    {
        foreach (Vector2 offset in offsets)
        {
            Vector2Int targetPos = centerPosition + new Vector2Int(Mathf.RoundToInt(offset.x), Mathf.RoundToInt(offset.y));
            if (gridCells.ContainsKey(targetPos))
            {
                Destroy(gridCells[targetPos].gameObject);
                gridCells.Remove(targetPos);
            }
        }
    }
    
    public void ClearHighlights()
    {
        foreach (var cell in gridCells.Values)
        {
            cell.SetHighlight(false);
        }
    }

    public void HighlightCells(Vector2Int centerPosition, List<Vector2> offsets)
    {
        ClearHighlights();
        
        foreach (Vector2 offset in offsets)
        {
            Vector2Int targetPos = centerPosition + new Vector2Int(Mathf.RoundToInt(offset.x), Mathf.RoundToInt(offset.y));
            
            if (gridCells.ContainsKey(targetPos))
            {
                gridCells[targetPos].SetHighlight(true);
            }
        }
    }
}