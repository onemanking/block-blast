using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlock : Block
{
    private SpriteRenderer _SpriteRenderer;
    private BlockColor _Color;

    protected override void Start()
    {
        base.Start();
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

    internal override List<Block> GetNearBlocks() => GetNearBlocks(_Color);
    internal override List<Block> GetNearBlocks(BlockColor _color)
    {
        List<Block> result = new List<Block>();
        if(_Color == _color && !IsChecking)
        {
            IsChecking = true;
            result.Add(this);

            if (GetBlock(Vector2.up) != null)
            {
                result.AddRange(GetBlock(Vector2.up).GetNearBlocks(_color));
            }
            if (GetBlock(Vector2.down) != null)
            {
                result.AddRange(GetBlock(Vector2.down).GetNearBlocks(_color));
            }
            if (GetBlock(Vector2.left) != null)
            {
                result.AddRange(GetBlock(Vector2.left).GetNearBlocks(_color));
            }
            if (GetBlock(Vector2.right) != null)
            {
                result.AddRange(GetBlock(Vector2.right).GetNearBlocks(_color));
            }
        }
        return result;
    }
}
