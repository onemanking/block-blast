using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlock : Block
{
    private SpriteRenderer _SpriteRenderer;
    private BlockColor _Color;

    void Start()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();

        _Color = (BlockColor)Random.Range(0, 4);
        switch (_Color)
        {
            case BlockColor.Red:
                _SpriteRenderer.color = Color.red;
                break;
            case BlockColor.Blue:
                _SpriteRenderer.color = Color.blue;
                break;
            case BlockColor.Green:
                _SpriteRenderer.color = Color.green;
                break;
            case BlockColor.Yellow:
                _SpriteRenderer.color = Color.yellow;
                break;
        }
    }

    internal override List<Block> GetBlocks() => GetBlocks(_Color);
    internal override List<Block> GetBlocks(BlockColor _color)
    {
        List<Block> result = new List<Block>();
        if (_Color == _color && !IsChecking)
        {
            IsChecking = true;
            result.Add(this);

            if (GetBlock(Vector2.up) != null)
            {
                result.AddRange(GetBlock(Vector2.up).GetBlocks(_color));
            }
            if (GetBlock(Vector2.down) != null)
            {
                result.AddRange(GetBlock(Vector2.down).GetBlocks(_color));
            }
            if (GetBlock(Vector2.left) != null)
            {
                result.AddRange(GetBlock(Vector2.left).GetBlocks(_color));
            }
            if (GetBlock(Vector2.right) != null)
            {
                result.AddRange(GetBlock(Vector2.right).GetBlocks(_color));
            }
        }
        return result;
    }
}
