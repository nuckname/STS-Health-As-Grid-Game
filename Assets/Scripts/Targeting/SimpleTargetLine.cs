using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SimpleTargetLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public CardDataSO currentCardData;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        
        // Ensure the line always has exactly two points (start and end)
        lineRenderer.positionCount = 2;
        lineRenderer.useWorldSpace = true;
    }

    private void Update()
    {
        // Gets position of the emitter point.
        lineRenderer.SetPosition(0, transform.position);

        // Sets the end point directly to the current mouse position.
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lineRenderer.SetPosition(1, mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        
        if (hit.collider != null)
        {
            GridCell hoveredCell = hit.collider.GetComponent<GridCell>();
            if (hoveredCell != null && currentCardData != null && BattleGrid.Instance != null)
            {
                BattleGrid.Instance.ValidateAndHighlightCells(hoveredCell.gridPosition, currentCardData.girdPositionsForCards.gridPositons);
                return;
            }
        }

        if (BattleGrid.Instance != null)
        {
            BattleGrid.Instance.ClearHighlights();
        }
    }

    public bool TryPlayCard()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        
        if (hit.collider != null)
        {
            GridCell hoveredCell = hit.collider.GetComponent<GridCell>();
            if (hoveredCell != null && currentCardData != null && BattleGrid.Instance != null)
            {
                bool isValid = BattleGrid.Instance.ValidateAndHighlightCells(hoveredCell.gridPosition, currentCardData.girdPositionsForCards.gridPositons);
                
                if (isValid)
                {
                    BattleGrid.Instance.RemoveCells(hoveredCell.gridPosition, currentCardData.girdPositionsForCards.gridPositons);
                    return true;
                }
            }
        }
        return false;
    }
    
    private void OnDestroy()
    {
        if (BattleGrid.Instance != null)
        {
            BattleGrid.Instance.ClearHighlights();
        }
    }
}