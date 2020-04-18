using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
[RequireComponent(typeof(RectTransform))]
public class UIDynamicCellSize : MonoBehaviour
{
    [SerializeField][Range(1, 4)] int horizontalCells = 3;
    [SerializeField][Range(1, 4)] int verticalCells = 3;

    RectTransform rectTransform;
    GridLayoutGroup gridLayoutGroup;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();

        float containerWidth = rectTransform.rect.width; 
        float horizontalPadding = gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;
        float horizontalSpacing = gridLayoutGroup.spacing.x * (horizontalCells - 1);
        float cellSizeWidth = (containerWidth - horizontalPadding - horizontalSpacing) / horizontalCells;

        float containerHeight = rectTransform.rect.height;
        float verticalPadding = gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom;
        float verticalSpacing = gridLayoutGroup.spacing.y * (verticalCells - 1);
        float cellSizeHeight = (containerHeight - verticalPadding - verticalSpacing) / verticalCells;

        Vector2 dynamicCellSize = new Vector2(cellSizeWidth, cellSizeHeight);
        gridLayoutGroup.cellSize = dynamicCellSize;
    }
}