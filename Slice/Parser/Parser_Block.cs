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
    
    private BlockNode ParseBlock(ParseBlockOption option, ParseStatementOption statementOption = ParseStatementOption.None)
    {
        var block = new BlockNode();
        
        if (option is ParseBlockOption.None)
        {
            while (HasNext)
            {
                if (ParseStatement(statementOption) is { } result)
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

            if (CurrentToken.Type == TokenType.BLOCK_CLOSE)
            {
                return block;
            }
            
            while (HasNext && CurrentToken.Type != TokenType.BLOCK_CLOSE)
            {
                if (ParseStatement(statementOption) is { } result)
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