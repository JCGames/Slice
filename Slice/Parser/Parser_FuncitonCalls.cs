using Slice.Models;
using Slice.Models.Nodes;

namespace Slice.Parser;

public partial class Parser
{
    private FunctionCallNode ParseFunctionCall()
    {
        var identifier = new IdentifierNode(CurrentToken.Value)
        {
            Meta = CurrentToken.Meta
        };

        var functionCall = new FunctionCallNode(identifier);
        
        MoveNext();

        if (CurrentToken.Type != TokenType.PARAN_OPEN)
        {
            Diagnostics.LogError(CurrentToken.Meta, "Missing open parenthesis.");
        }
        
        MoveNext();

        var firstIteration = true;
        
        while (HasNext && CurrentToken.Type != TokenType.PARAN_CLOSE)
        {
            if (!firstIteration)
            {
                MoveNext();
            }

            var expression = ParseExpression();
            
            functionCall.Arguments.Add(expression);

            if (CurrentToken.Type is not TokenType.COMMA and not TokenType.PARAN_CLOSE)
            {
                Diagnostics.LogError(CurrentToken.Meta, "Missing comma or closed parenthesis.");
            }
            
            firstIteration = false;
        }

        if (CurrentToken.Type != TokenType.PARAN_CLOSE)
        {
            Diagnostics.LogError(CurrentToken.Meta, "Missing close parenthesis.");
        }
        
        MoveNext();

        return functionCall;
    }
}