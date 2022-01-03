using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BlockManager : MonoBehaviour
{
    private const float _COLLAPE_DURATION = 0.5f;

    internal static BlockManager Instance { get; private set; }


    [Header("Block Prefabs")]
    [SerializeField] private ColorBlock m_ColorPrefab;
    [SerializeField] private BombBlock m_BombPrefab;

    [Header("Block Spawning")]
    [SerializeField] private Transform m_BlockParent;

    internal Block[,] Blocks;

    internal int Row => PanelManager.Instance.GridPositions.GetLength(0);
    internal int Column => PanelManager.Instance.GridPositions.GetLength(1);

    void Awake() => Instance = this;

    void Start()
    {
        PanelManager.OnInitPanelCompleted()
            .Subscribe(_ =>
            {
                foreach (Transform child in m_BlockParent)
                {
                    Destroy(child.gameObject);
                }

                Blocks = new Block[Row, Column];

                CreateBlocks();
            }).AddTo(this);
    }

    private void CreateBlocks()
    {
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                Blocks[i, j] = CreateBlock(m_ColorPrefab, i, j);
            }
        }
    }

    private Block CreateBlock(Block _blockPrefab, int _row, int _column)
    {
        var block = Instantiate(_blockPrefab, PanelManager.Instance.GridPositions[_row, _column], Quaternion.identity, m_BlockParent) as Block;
        block.name = $"Block[{ _row },{ _column }]";
        block.SetIndex(new Vector2Int(_row, _column));
        return block;
    }

    internal void RemoveBlock(Block _block)
    {
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                if (Blocks[i, j] == _block)
                {
                    Blocks[i, j] = null;
                    Destroy(_block.gameObject);
                }
            }
        }
    }

    internal void CollapseBlocks()
    {
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                if (Blocks[i, j] == null)
                {
                    foreach (int k in Enumerable.Range(j + 1, Column))
                    {
                        if (k < Column && Blocks[i, k] != null)
                        {
                            Blocks[i, k].Move(_COLLAPE_DURATION, new Vector2(i, j));
                            Blocks[i, j] = Blocks[i, k];
                            Blocks[i, j].name = $"Block[{ i },{ j }]";
                            Blocks[i, j].SetIndex(new Vector2Int(i, j));
                            Blocks[i, k] = null;
                            break;
                        }
                    }
                }
            }
        }
    }

    internal void RefillBlocks(int _blastCount)
    {
        bool spawnSpecialBlockAlready = false;
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Column; j++)
            {
                if (Blocks[i, j] == null)
                {
                    Block prefab = null;
                    if (!spawnSpecialBlockAlready)
                    {
                        prefab = _blastCount >= 10 ? m_BombPrefab : _blastCount >= 6 ? m_BombPrefab : m_ColorPrefab as Block;
                        spawnSpecialBlockAlready = true;
                    }
                    else
                    {
                        prefab = m_ColorPrefab;
                    }

                    var block = CreateBlock(prefab, i, j);
                    var topPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                    Blocks[i, j] = block;
                    block.transform.position = new Vector2(block.transform.position.x, block.transform.position.y + topPos.y);
                    block.Move(_COLLAPE_DURATION, new Vector2(i, j));
                }
            }
        }
    }

    internal List<Block> GetBlocksAtRow(int _row)
    {
        List<Block> result = new List<Block>();
        for (int i = 0; i < Column; i++)
        {
            if (Blocks[_row, i] != null)
            {
                result.Add(Blocks[_row, i]);
            }
        }
        return result;
    }

    internal List<Block> GetBlocksAtColumn(int _column)
    {
        List<Block> result = new List<Block>();
        for (int i = 0; i < Row; i++)
        {
            if (Blocks[i, _column] != null)
            {
                result.Add(Blocks[i, _column]);
            }
        }
        return result;
    }
}
