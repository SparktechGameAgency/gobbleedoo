using System.Collections.Generic;
using UnityEngine;

public class GridRenderer : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject cellVisualPrefab;
    public RectTransform gridParent;

    private Dictionary<Vector2Int, GameObject> activeCells
        = new Dictionary<Vector2Int, GameObject>();

    public void PlaceVisual(BlockData blockData, Vector2Int origin)
    {
        foreach (var cell in blockData.cells)
        {
            Vector2Int pos = new Vector2Int(
                origin.x + cell.x,
                origin.y + cell.y
            );

            GameObject cellObj =
                Instantiate(cellVisualPrefab, gridParent);

            RectTransform rect =
                cellObj.GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector2(
                pos.x * gridManager.cellSize,
                -pos.y * gridManager.cellSize
            );

            activeCells[pos] = cellObj;
        }
    }

    public void ClearRow(int row)
    {
        List<Vector2Int> toRemove = new List<Vector2Int>();

        foreach (var kvp in activeCells)
        {
            if (kvp.Key.y == row)
            {
                Destroy(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var key in toRemove)
            activeCells.Remove(key);
    }

    public void ClearColumn(int col)
    {
        List<Vector2Int> toRemove = new List<Vector2Int>();

        foreach (var kvp in activeCells)
        {
            if (kvp.Key.x == col)
            {
                Destroy(kvp.Value);
                toRemove.Add(kvp.Key);
            }
        }

        foreach (var key in toRemove)
            activeCells.Remove(key);
    }
}