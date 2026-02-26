using UnityEngine;
using UnityEngine.EventSystems;

public class BlockView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BlockData blockData;
    public GameObject cellPrefab;
    public float cellSize = 100f;

    private Vector2 startPosition;
    private Canvas canvas;
    public GridManager gridManager;
    private BlockTrayManager trayManager;
    private GridRenderer gridRenderer;

    private Vector2 dragOffset;     // before step 7 

    public void Initialize(BlockData data, GridManager grid)
    {
        blockData = data;
        gridManager = grid;
        trayManager = GetComponentInParent<BlockTrayManager>();
        //gridRenderer = FindObjectOfType<GridRenderer>();
        gridRenderer = Object.FindFirstObjectByType<GridRenderer>();

        canvas = GetComponentInParent<Canvas>();

        GenerateCells();
    }

    void GenerateCells()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        foreach (var cell in blockData.cells)
        {
            GameObject cellObj = Instantiate(cellPrefab, transform);
            RectTransform rect = cellObj.GetComponent<RectTransform>();

            rect.anchoredPosition = new Vector2(
                cell.x * cellSize,
                cell.y * cellSize
            );
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        RectTransform rect = GetComponent<RectTransform>();
        dragOffset = rect.anchoredPosition - localPoint;

        startPosition = rect.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        GetComponent<RectTransform>().anchoredPosition =
            localPoint + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        TryPlace(eventData.position);
    }

    void TryPlace(Vector2 screenPosition)
    {
        Vector2Int gridPos = gridManager.WorldToGrid(screenPosition);

        if (PlacementValidator.CanPlace(blockData, gridPos, gridManager))
        {
            PlacementValidator.Place(blockData, gridPos, gridManager);

            gridRenderer.PlaceVisual(blockData, gridPos);       // step 6.5

            GameManager.Instance.OnBlockPlaced();
            //CheckForLineClear();
            //GameManager.Instance.CheckGameOver();

            //SnapToGrid(gridPos);

            //GetComponent<CanvasGroup>().blocksRaycasts = false;
            //enabled = false;
            trayManager.NotifyBlockUsed(this);
            Destroy(gameObject);
        }
        else
        {
            //transform.position = startPosition;
            GetComponent<RectTransform>().anchoredPosition = startPosition;
        }
    }

    //void CheckForLineClear()
    //{
    //    var completedRows = gridManager.GetCompletedRows();
    //    var completedColumns = gridManager.GetCompletedColumns();

    //    foreach (int row in completedRows)
    //    {
    //        gridManager.ClearRow(row);
    //        gridRenderer.ClearRow(row);
    //    }

    //    foreach (int col in completedColumns)
    //    {
    //        gridManager.ClearColumn(col);
    //        gridRenderer.ClearColumn(col);
    //    }
    //}
    //void CheckForLineClear()
    //{
    //    var completedRows = gridManager.GetCompletedRows();
    //    var completedColumns = gridManager.GetCompletedColumns();

    //    if (completedRows.Count > 0 || completedColumns.Count > 0)
    //    {
    //        GameManager.Instance.HandleLineClear(completedRows, completedColumns);
    //    }
    //}

    void SnapToGrid(Vector2Int gridPos)
    {
        Vector2 anchoredPos = new Vector2(
            gridPos.x * gridManager.cellSize,
            -gridPos.y * gridManager.cellSize
        );

        //transform.anchoredPosition = anchoredPos;
        GetComponent<RectTransform>().anchoredPosition = anchoredPos;
    }
}