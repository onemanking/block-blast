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

    [Header("Block Spawning")]
    [SerializeField] private Transform m_BlockParent;

    internal Block[,] Blocks;

    private int _Row => PanelManager.Instance.GridPositions.GetLength(0);
    private int _Column => PanelManager.Instance.GridPositions.GetLength(1);

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

                Blocks = new Block[_Row, _Column];

                CreateBlocks();
            }).AddTo(this);
    }

    private void CreateBlocks()
    {
        for (int i = 0; i < _Row; i++)
        {
            for (int j = 0; j < _Column; j++)
            {
                Blocks[i, j] = CreateBlock(i, j);
            }
        }
    }

    private Block CreateBlock(int _row, int _column)
    {
        var block = Instantiate(m_ColorPrefab, PanelManager.Instance.GridPositions[_row, _column], Quaternion.identity, m_BlockParent) as Block;
        block.name = $"Block[{ _row },{ _column }]";
        return block;
    }

    internal void RemoveBlock(Block _block)
    {
        for (int i = 0; i < _Row; i++)
        {
            for (int j = 0; j < _Column; j++)
            {
                if(Blocks[i, j] == _block)
                {
                    Blocks[i, j] = null;
                    Destroy(_block.gameObject);
                }
            }
        }
    }

    internal void CollapseBlocks()
    {
        for (int i = 0; i < _Row; i++)
        {
            for (int j = 0; j < _Column; j++)
            {
                if (Blocks[i, j] == null)
                {
                    foreach (int k in Enumerable.Range(j + 1, _Column))
                    {
                        if (k < _Column && Blocks[i, k] != null)
                        {
                            Blocks[i, k].Move(_COLLAPE_DURATION, new Vector2(i, j));
                            Blocks[i, j] = Blocks[i, k];
                            Blocks[i, j].name = $"Block[{ i },{ j }]";
                            Blocks[i, k] = null;
                            break;
                        }
                    }
                }
            }
        }
    }

    internal void RefillBlocks()
    {
        for (int i = 0; i < _Row; i++)
        {
            for (int j = 0; j < _Column; j++)
            {
                if (Blocks[i, j] == null)
                {
                    var block = CreateBlock(i, j);
                    var topPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                    Blocks[i, j] = block;
                    block.transform.position = new Vector2(block.transform.position.x, block.transform.position.y + topPos.y);
                    block.Move(_COLLAPE_DURATION, new Vector2(i, j));
                }
            }
        }
    }
}
