using UnityEngine;
using UnityEngine.UI;

public class DynamicGridScaler : MonoBehaviour
{
    private int rows = 2;
    private int columns = 2;
    private Vector2 spacing = new Vector2(10f, 10f);
    private Vector2 padding = new Vector2(20f, 20f);

    private RectTransform container;

    private void Start()
    {
      container = GetComponent<RectTransform>();
    }

    public void UpdateGrid(int r, int c, float space, float paddingValue)
    {
        rows = r;
        columns = c;
        spacing = new Vector2(space, space);
        padding = new Vector2(paddingValue, paddingValue);

        Vector2 size = container.rect.size;

        float cellWidth = (size.x - (padding.x * 2) - (spacing.x * (columns - 1))) / columns;
        float cellHeight = (size.y - (padding.y * 2) - (spacing.y * (rows - 1))) / rows;
        float cellSize = Mathf.Min(cellWidth, cellHeight); // Keep square cards
        container.GetComponent<GridLayoutGroup>().padding = new RectOffset((int)padding.x, (int)padding.x, (int)padding.y, (int)padding.y);
        container.GetComponent<GridLayoutGroup>().cellSize = new Vector2(cellSize, cellSize);
        container.GetComponent<GridLayoutGroup>().spacing = spacing;
        container.GetComponent<GridLayoutGroup>().constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        container.GetComponent<GridLayoutGroup>().constraintCount = columns;
    }
}
