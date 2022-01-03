using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class BombBlock : Block
{
    internal override List<Block> GetBlocks()
    {
        List<Block> result = new List<Block>();
        if (!IsChecking)
        {
            IsChecking = true;
            result.Add(this);
            foreach (var i in BlockManager.Instance.GetBlocksAtRow(Index.x))
                if (i != null) result.AddRange(i.GetBlocks());

            foreach (var i in BlockManager.Instance.GetBlocksAtColumn(Index.y))
                if (i != null) result.AddRange(i.GetBlocks());
        }
        return result;
    }

    internal override List<Block> GetBlocks(BlockColor _color) => new List<Block>();
}
