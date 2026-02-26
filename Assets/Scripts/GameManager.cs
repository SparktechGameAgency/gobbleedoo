using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GridManager gridManager;
    public GridRenderer gridRenderer;

    //public BlockTrayManager trayManager;

    public GameObject lineClearPopup;
    public GameObject gameOverPopup;

    private List<int> pendingRows = new List<int>();
    private List<int> pendingColumns = new List<int>();

    void Awake()
    {
        Instance = this;
    }

    public void OnBlockPlaced()
    {
        pendingRows = gridManager.GetCompletedRows();
        pendingColumns = gridManager.GetCompletedColumns();

        if (pendingRows.Count > 0 || pendingColumns.Count > 0)
        {
            lineClearPopup.SetActive(true);
        }
        else
        {
            CheckGameOver();
        }
    }

    public void ConfirmLineClear()
    {
        foreach (int row in pendingRows)
        {
            gridManager.ClearRow(row);
            gridRenderer.ClearRow(row);
        }

        foreach (int col in pendingColumns)
        {
            gridManager.ClearColumn(col);
            gridRenderer.ClearColumn(col);
        }

        lineClearPopup.SetActive(false);

        CheckGameOver();
    }

    void CheckGameOver()
    {
        var tray = FindObjectOfType<BlockTrayManager>();

        foreach (var block in tray.GetActiveBlocks())
        {
            for (int x = 0; x < gridManager.width; x++)
            {
                for (int y = 0; y < gridManager.height; y++)
                {
                    if (PlacementValidator.CanPlace(block.blockData,
                                                    new Vector2Int(x, y),
                                                    gridManager))
                        return;
                }
            }
        }

        gameOverPopup.SetActive(true);
    }
}