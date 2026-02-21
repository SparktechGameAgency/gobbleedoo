using System.Collections.Generic;
using UnityEngine;

public class BlockTrayManager : MonoBehaviour
{
    public static BlockTrayManager Instance;

    public Transform trayParent;
    public BlockView blockPrefab;
    public List<BlockData> availableBlocks;

    private int remainingBlocks;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnNewSet()
    {
        remainingBlocks = 3;

        for (int i = 0; i < 3; i++)
        {
            BlockData randomBlock =
                availableBlocks[Random.Range(0, availableBlocks.Count)];

            BlockView block =
                Instantiate(blockPrefab, trayParent);

            //block.Initialize(randomBlock, FindObjectOfType<GridManager>());
        }
    }

    public void NotifyBlockUsed()
    {
        remainingBlocks--;

        if (remainingBlocks <= 0)
            SpawnNewSet();
    }
}