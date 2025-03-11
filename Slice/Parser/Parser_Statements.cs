using Slice.Models;
using Slice.Models.Nodes;
using Slice.Models.Nodes.BinaryOperators;

namespace Slice.Parser;

public partial class Parser
{
    private Node ParseStatement()
    {
        // Function
        if (CurrentToken.Type is TokenType.KEYWORD && CurrentToken.Value == "fn")
        {
            return ParseFunction();
        }
        
        // Assignment
        if (CurrentToken.Type is TokenType.IDENTIFIER && PeekNext().Type is TokenType.ASSIGNMENT)
        {
            var identifier = new IdentifierNode(CurrentToken.Value)
            {
                Meta = CurrentToken.Meta
            };

            MoveNext();
            
            var assignment = new AssignmentNode()
            {
                Meta = CurrentToken.Meta
            };
            
            MoveNext();
            
            assignment.Value.LeftChild = identifier;
            assignment.Value.RightChild = ParseExpression();

            return assignment;
        }
        
        // Variable declaration and assignment
        if ((CurrentToken.Type is TokenType.TYPE_KEYWORD && PeekNext().Type is TokenType.IDENTIFIER) ||
            (CurrentToken.Type is TokenType.IDENTIFIER && PeekNext().Type is TokenType.IDENTIFIER))
        {   
            var type = new TypeNode(CurrentToken.Value)
            {
                Meta = CurrentToken.Meta
            };

            MoveNext();
            
            var identifier = new IdentifierNode(CurrentToken.Value)
            {
                Meta = CurrentToken.Meta
            };

            MoveNext();

            if (CurrentToken.Type is TokenType.ASSIGNMENT)
            {
                var assignment = new AssignmentNode()
                {
                    Meta = CurrentToken.Meta
                };
                
                assignment.Value.LeftChild = new VariableDeclaration(type, identifier);
             
                MoveNext();
                assignment.Value.RightChild = ParseExpression();
                
                return assignment;
            }

            return new VariableDeclaration(type, identifier);
        }
        
        // Expression
        if (CurrentToken.Type is TokenType.INTEGER
            or TokenType.DECIMAL
            or TokenType.BOOLEAN
            or TokenType.STRING
            or TokenType.IDENTIFIER
            or TokenType.PARAN_OPEN)
        {
            return ParseExpression();
        }

        Diagnostics.LogError(CurrentToken.Meta, $"Bad statement {CurrentToken.Value}.");
        return new ErrorNode();
    }
}