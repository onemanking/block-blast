using System.Collections.Generic;

public class DiscoBlock : ColorBlock
{
    internal override List<Block> GetBlocks()
    {
        List<Block> result = new List<Block>();
        if (!IsChecking)
        {
            IsChecking = true;
            result.Add(this);
            foreach (var i in BlockManager.Instance.GetBlocksWithColor(Color))
                if (i != null) result.AddRange(i.GetBlocks());
        }
        return result;
    }
}
