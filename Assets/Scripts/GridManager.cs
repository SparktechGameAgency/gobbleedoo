using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 8;

    public float cellSize = 100f;   // Must match BlockView cellSize
    public RectTransform gridArea;  // Assign in Inspector

    private bool[,] grid;

    [SerializeField] private Canvas canvas;

    void Awake()
    {
        grid = new bool[width, height];
    }

    void Start()
    {
        CalculateCellSize();
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
            canvas.worldCamera,
            out Vector2 localPoint
        );

        int x = Mathf.FloorToInt(localPoint.x / cellSize);
        int y = Mathf.FloorToInt(-localPoint.y / cellSize);

        return new Vector2Int(x, y);
    }

    public List<int> GetCompletedRows()
    {
        List<int> completed = new List<int>();

        for (int y = 0; y < height; y++)
        {
            bool full = true;

            for (int x = 0; x < width; x++)
            {
                if (!grid[x, y])
                {
                    full = false;
                    break;
                }
            }

            if (full)
                completed.Add(y);
        }

        return completed;
    }

    public List<int> GetCompletedColumns()
    {
        List<int> completed = new List<int>();

        for (int x = 0; x < width; x++)
        {
            bool full = true;

            for (int y = 0; y < height; y++)
            {
                if (!grid[x, y])
                {
                    full = false;
                    break;
                }
            }

            if (full)
                completed.Add(x);
        }

        return completed;
    }

    public void ClearRow(int row)
    {
        for (int x = 0; x < width; x++)
        {
            grid[x, row] = false;
        }
    }

    public void ClearColumn(int column)
    {
        for (int y = 0; y < height; y++)
        {
            grid[column, y] = false;
        }
    }

    public void CalculateCellSize()
    {
        float boardWidth = gridArea.rect.width;
        cellSize = boardWidth / width;
    }
}