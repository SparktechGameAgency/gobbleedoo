using UnityEngine;
using UnityEngine.EventSystems;

public class BlockView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public BlockData blockData;
    public GameObject cellPrefab;
    public float cellSize = 100f;

    private Vector3 startPosition;
    private Canvas canvas;
    public GridManager gridManager;

    void Start()
    {
        if (blockData != null && gridManager != null)
        {
            Initialize(blockData, gridManager);
        }
    }

    public void Initialize(BlockData data, GridManager grid)
    {
        blockData = data;
        gridManager = grid;

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
        startPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3)eventData.delta;
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

            SnapToGrid(gridPos);

            GetComponent<CanvasGroup>().blocksRaycasts = false;
            enabled = false;
        }
        else
        {
            transform.position = startPosition;
        }
    }

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