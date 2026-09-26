using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class GirdPositionsForCards
{
    public enum NameOfGridPosition
    {
        x_1_by_y_1,
        
        x_1_by_y_2,
        x_1_by_y_3,
        
        x_2_by_y_1,
        x_3_by_y_1,
        
        x_2_by_y_2,
    }
    
    public NameOfGridPosition gridPositionType;
    
    // Changing this to a getter property means you never have to set the list manually.
    // It will automatically output the correct set based purely on the dropdown choice.
    public List<Vector2> gridPositons 
    {
        get
        {
            switch (gridPositionType)
            {
                case NameOfGridPosition.x_1_by_y_1:
                    return new List<Vector2> { new Vector2(0, 0) };
                case NameOfGridPosition.x_1_by_y_2:
                    return new List<Vector2> { new Vector2(0, 0), new Vector2(0, 1) };
                case NameOfGridPosition.x_1_by_y_3:
                    return new List<Vector2> { new Vector2(0, 0), new Vector2(0, 1), new Vector2(0, 2) };
                case NameOfGridPosition.x_2_by_y_1:
                    return new List<Vector2> { new Vector2(0, 0), new Vector2(1, 0) };
                case NameOfGridPosition.x_3_by_y_1:
                    return new List<Vector2> { new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0) };
                case NameOfGridPosition.x_2_by_y_2:
                    return new List<Vector2> { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1) };
                default:
                    return new List<Vector2>();
            }
        }
    }
}

[CreateAssetMenu(fileName = "NewCardData", menuName = "Scriptable Objects/Card Data")]
public class CardDataSO : ScriptableObject
{
    public GirdPositionsForCards girdPositionsForCards;
    public int cardCost = 1;

    public Sprite background;
    public Sprite image;
}