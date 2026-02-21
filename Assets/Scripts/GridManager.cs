using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;

    public float cellSize = 100f;   // Must match BlockView cellSize
    public RectTransform gridArea;  // Assign in Inspector

    private bool[,] grid;

    void Awake()
    {
        grid = new bool[width, height];
    }

    public bool IsInsideGrid(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    public bool IsCellOccupied(int x, int y)
    {
        return grid[x, y];
    }

    public void SetCell(int x, int y, bool value)
    {
        grid[x, y] = value;
    }

    public Vector2Int WorldToGrid(Vector2 worldPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gridArea,
            worldPosition,
            null,
            out Vector2 localPoint
        );

        int x = Mathf.FloorToInt(localPoint.x / cellSize);
        int y = Mathf.FloorToInt(-localPoint.y / cellSize);

        return new Vector2Int(x, y);
    }
}