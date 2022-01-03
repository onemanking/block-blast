using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public abstract class Block : MonoBehaviour
{
    protected bool IsChecking;

    protected virtual void Start()
    {

    }

    protected void Blast()
    {
        BlockManager.Instance.RemoveBlock(this);
    }

    private void OnMouseDown()
    {
        foreach (var b in BlockManager.Instance.Blocks)
        {
            if (b != null)
                b.ResetChecking();
        }
        var blocks = GetNearBlocks();
        if (blocks.Count > 1)
        {
            foreach (var block in blocks)
                block.Blast();
        }

        BlockManager.Instance.CollapseBlocks();
        BlockManager.Instance.RefillBlocks();
    }

    internal abstract List<Block> GetNearBlocks();
    internal abstract List<Block> GetNearBlocks(BlockColor _color);

    protected Block GetBlock(Vector2 _dir)
    {
        var hit = Physics2D.Raycast(transform.position + (Vector3)_dir * 0.5f, _dir, 0.5f);
        if (hit.collider != null && hit.collider.GetComponent<Block>() != null)
        {
            Debug.DrawRay(transform.position + (Vector3)_dir * 0.5f, _dir * 0.5f, Color.white, 1f);
            return hit.collider.GetComponent<Block>();
        }
        return null;
    }

    internal void ResetChecking() => IsChecking = false;

    internal void Move(float _duration, Vector2 _pos)
    {
        transform.LerpLocalPosition(_duration, _pos).Subscribe().AddTo(this);
    }
}
