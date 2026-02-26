using System.Collections.Generic;
using UnityEngine;

public class BlockTrayManager : MonoBehaviour
{
    public BlockView blockPrefab;
    public List<BlockData> availableBlocks;
    public GridManager gridManager;

    private List<BlockView> activeBlocks = new List<BlockView>();

    void Start()
    {
        SpawnNewSet();
    }

    public void SpawnNewSet()
    {
        ClearTray();

        for (int i = 0; i < 3; i++)
        {
            BlockData randomBlock =
                availableBlocks[Random.Range(0, availableBlocks.Count)];

            BlockView block =
                Instantiate(blockPrefab, transform);

            block.Initialize(randomBlock, gridManager);

            activeBlocks.Add(block);
        }
    }

    public void NotifyBlockUsed(BlockView block)
    {
        activeBlocks.Remove(block);

        if (activeBlocks.Count == 0)
        {
            SpawnNewSet();
        }
    }

    void ClearTray()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        activeBlocks.Clear();
    }

    public List<BlockView> GetActiveBlocks()
    {
        return activeBlocks;
    }
}