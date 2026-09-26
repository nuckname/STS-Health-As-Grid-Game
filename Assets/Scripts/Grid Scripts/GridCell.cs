using UnityEngine;

public class GridCell : MonoBehaviour
{
    public Vector2Int gridPosition;
    private SpriteRenderer spriteRenderer;
    
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.red;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = defaultColor;
    }

    public void SetHighlight(bool isHighlighted)
    {
        spriteRenderer.color = isHighlighted ? highlightColor : defaultColor;
    }
}