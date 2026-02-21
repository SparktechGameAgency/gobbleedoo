using UnityEngine;

[CreateAssetMenu(fileName = "BlockData", menuName = "Scriptable Objects/BlockData")]
public class BlockData : ScriptableObject
{
    public Vector2Int[] cells;
}
