using Slice.Models;
using Slice.Models.Nodes;

namespace Slice.Parser;

public partial class Parser
{
    private FunctionNode ParseFunction()
    {
        MoveNext();

        if (CurrentToken.Type != TokenType.IDENTIFIER)
        {
            Diagnostics.LogError(CurrentToken.Meta, "Missing function name.");
        }

        var identifier = new IdentifierNode(CurrentToken.Value)
        {
            Meta = CurrentToken.Meta,
        };
        
        var function = new FunctionNode(identifier, null, new BlockNode());
        
        MoveNext();

        function.Parameters = ParseParameters();

        if (CurrentToken.Type == TokenType.COLON)
        {
            MoveNext();

            if (CurrentToken.Type is not TokenType.IDENTIFIER and not TokenType.TYPE_KEYWORD)
            {
                Diagnostics.LogError(CurrentToken.Meta, "Missing return type.");
            }

            function.ReturnType = new TypeNode(CurrentToken.Value);
            
            MoveNext();
        }

        if (CurrentToken.Type != TokenType.BLOCK_OPEN)
        {
            Diagnostics.LogError(CurrentToken.Meta, "Missing open bracket.");
        }

        function.Body = ParseBlock(ParseBlockOption.Brackets, ParseStatementOption.InFunction);
        
        MoveNext();
        
        return function;
    }

    private List<ParameterNode> ParseParameters()
    {
        var parameters = new List<ParameterNode>();
        var nameSet = new HashSet<string>();
        
        if (CurrentToken.Type != TokenType.PARAN_OPEN)
        {
            Diagnostics.LogError(CurrentToken.Meta, "Missing open parenthesis.");
        }
        
        MoveNext();
        
        if (CurrentToken.Type == TokenType.PARAN_CLOSE)
        {
            MoveNext();
            return parameters;
        }

        var firstIteration = true;
        
        while (HasNext && CurrentToken.Type != TokenType.PARAN_CLOSE)
        {
            if (!firstIteration)
            {
                MoveNext();
            }
            
            if (CurrentToken.Type != TokenType.IDENTIFIER)
            {
                Diagnostics.LogError(CurrentToken.Meta, "Missing parameter name.");
            }

            var identifier = new IdentifierNode(CurrentToken.Value)
            {
                Meta = CurrentToken.Meta,
            };

            if (nameSet.Contains(identifier.Value))
            {
                Diagnostics.LogError(CurrentToken.Meta, "A parameter with the same name already exists.");
            }
            
            nameSet.Add(identifier.Value);
            
            MoveNext();

            if (CurrentToken.Type != TokenType.COLON || PeekNext().Type is not TokenType.TYPE_KEYWORD and not TokenType.IDENTIFIER)
            {
                Diagnostics.LogError(CurrentToken.Meta, "Missing parameter's type declaration.");
            }
            
            MoveNext();
            
            parameters.Add(new ParameterNode(identifier, new TypeNode(CurrentToken.Value)));
            
            MoveNext();
            
            if (CurrentToken.Type is not TokenType.COMMA and not TokenType.PARAN_CLOSE)
            {
                Diagnostics.LogError(CurrentToken.Meta, "Missing a comma or closed parenthesis.");
            }

            firstIteration = false;
        }

        if (CurrentToken.Type is not TokenType.PARAN_CLOSE)
        {
            Diagnostics.LogError(CurrentToken.Meta, "Missing closed parenthesis.");
        }
        
        MoveNext();
        
        return parameters;
    }
}