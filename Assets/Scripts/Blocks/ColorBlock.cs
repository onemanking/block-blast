using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBlock : Block
{
    internal BlockColor Color { get; private set; }
    private SpriteRenderer _SpriteRenderer;

    void Start()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();

        Color = (BlockColor)Random.Range(0, 4);
        switch (Color)
        {
            case BlockColor.Red:
                _SpriteRenderer.color = UnityEngine.Color.red;
                break;
            case BlockColor.Blue:
                _SpriteRenderer.color = UnityEngine.Color.blue;
                break;
            case BlockColor.Green:
                _SpriteRenderer.color = UnityEngine.Color.green;
                break;
            case BlockColor.Yellow:
                _SpriteRenderer.color = UnityEngine.Color.yellow;
                break;
        }
    }

    internal override List<Block> GetBlocks() => GetBlocks(Color);
    internal override List<Block> GetBlocks(BlockColor _color)
    {
        List<Block> result = new List<Block>();
        if (Color == _color && !IsChecking)
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
