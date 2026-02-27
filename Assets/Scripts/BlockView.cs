using UnityEngine;
using UnityEngine.EventSystems;

public class BlockView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BlockData blockData;
    public GameObject cellPrefab;
    //public float cellSize = 100f;
    private float cellSize;

    private Vector2 startPosition;
    private Canvas canvas;
    public GridManager gridManager;
    private BlockTrayManager trayManager;
    private GridRenderer gridRenderer;

    private Vector2 dragOffset;     // before step 7 

    private RectTransform rectTransform;

    public void Initialize(BlockData data, GridManager grid)
    {
        blockData = data;
        gridManager = grid;

        cellSize = gridManager.cellSize;

        trayManager = GetComponentInParent<BlockTrayManager>();
        gridRenderer = Object.FindFirstObjectByType<GridRenderer>();

        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();

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
                -cell.y * cellSize
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

        dragOffset = rectTransform.anchoredPosition - localPoint;
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out Vector2 localPoint
        );

        rectTransform.anchoredPosition = localPoint + dragOffset;
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

            gridRenderer.PlaceVisual(blockData, gridPos);       

            GameManager.Instance.OnBlockPlaced();

            trayManager.NotifyBlockUsed(this);
            Destroy(gameObject);
        }
        else
        {
            rectTransform.anchoredPosition = startPosition;
        }
    }
}