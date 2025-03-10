using Slice.Models;
using Slice.Models.Nodes;

namespace Slice.Parser;

public partial class Parser
{
    private enum ParseBlockOption
    {
        None,
        Brackets,
    }
    
    private BlockNode ParseBlock(ParseBlockOption option)
    {
        var block = new BlockNode();
        
        if (option is ParseBlockOption.None)
        {
            while (HasNext)
            {
                if (ParseStatement() is { } result)
                {
                    block.Value.Add(result);
                }
            }
        }
        else if (option is ParseBlockOption.Brackets)
        {
            if (CurrentToken.Type != TokenType.BLOCK_OPEN)
            {
                Diagnostics.LogError(CurrentToken.Meta, "Missing open bracket.");
            }
            
            MoveNext();
            
            while (HasNext)
            {
                if (ParseStatement() is { } result)
                {
                    block.Value.Add(result);
                }   
            }
            
            if (CurrentToken.Type != TokenType.BLOCK_CLOSE)
            {
                Diagnostics.LogError(CurrentToken.Meta, "Missing close bracket.");
            }
            
            MoveNext();
        }

        return block;
    }
}