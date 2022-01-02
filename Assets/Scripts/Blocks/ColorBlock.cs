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
}
