using UnityEngine;

public static class PlacementValidator
{
    public static bool CanPlace(BlockData blockData,
                                Vector2Int origin,
                                GridManager grid)
    {
        foreach (var cell in blockData.cells)
        {
            int x = origin.x + cell.x;
            int y = origin.y + cell.y;

            if (!grid.IsInsideGrid(x, y))
                return false;

            if (grid.IsCellOccupied(x, y))
                return false;
        }

        return true;
    }

    public static void Place(BlockData blockData,
                             Vector2Int origin,
                             GridManager grid)
    {
        foreach (var cell in blockData.cells)
        {
            grid.SetCell(origin.x + cell.x,
                         origin.y + cell.y,
                         true);
        }
    }
}