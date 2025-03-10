using Slice.Models;
using Slice.Models.Nodes;
using Slice.Models.Nodes.BinaryOperators;
using Slice.Models.Nodes.ValueNodes;

namespace Slice.Parser;

/**
 * Logical
 * Equality
 * Relational
 * Term
 * Factor
 * Primary
 */

public partial class Parser
{
    private ExpressionNode ParseExpression()
    {
        return new ExpressionNode(ParseLogical());
    }
    
    private Node ParseLogical()
    {
        var left = ParseEquality();

        while (CurrentToken.Type is TokenType.AND or TokenType.OR)
        {   
            if (CurrentToken.Type is TokenType.AND)
            {
                MoveNext();
                var node = new AndNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParseEquality();
                left = node;
            }
            else if (CurrentToken.Type is TokenType.OR)
            {
                MoveNext();
                var node = new OrNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParseEquality();
                left = node;
            }
        }

        return left;
    }
    
    private Node ParseEquality()
    {
        var left = ParseRelational();

        while (CurrentToken.Type is TokenType.EQUALS or TokenType.NOT_EQUALS)
        {   
            if (CurrentToken.Type is TokenType.EQUALS)
            {
                MoveNext();
                var node = new EqualsNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParseRelational();
                left = node;
            }
            else if (CurrentToken.Type is TokenType.NOT_EQUALS)
            {
                MoveNext();
                var node = new NotEqualsNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParseRelational();
                left = node;
            }
        }

        return left;
    }

    private Node ParseRelational()
    {
        var left = ParseTerm();

        while (CurrentToken.Type is TokenType.GREATER_THAN 
               or TokenType.LESS_THAN
               or TokenType.GREATER_THAN_OR_EQUAL
               or TokenType.LESS_THAN_OR_EQUAL)
        {   
            if (CurrentToken.Type is TokenType.GREATER_THAN)
            {
                MoveNext();
                var node = new GreaterThanNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParseTerm();
                left = node;
            }
            else if (CurrentToken.Type is TokenType.LESS_THAN)
            {
                MoveNext();
                var node = new LessThanNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParseTerm();
                left = node;
            }
            else if (CurrentToken.Type is TokenType.GREATER_THAN_OR_EQUAL)
            {
                MoveNext();
                var node = new GreaterThanOrEqualToNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParseTerm();
                left = node;
            }
            else if (CurrentToken.Type is TokenType.LESS_THAN_OR_EQUAL)
            {
                MoveNext();
                var node = new LessThanOrEqualToNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParseTerm();
                left = node;
            }
        }

        return left;
    }
    
    private Node ParseTerm()
    {
        var left = ParseFactor();

        while (CurrentToken.Type is TokenType.ADDITION or TokenType.SUBTRACTION)
        {   
            if (CurrentToken.Type is TokenType.ADDITION)
            {
                MoveNext();
                var node = new AdditionNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParseFactor();
                left = node;
            }
            else if (CurrentToken.Type is TokenType.SUBTRACTION)
            {
                MoveNext();
                var node = new SubtractionNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParseFactor();
                left = node;
            }
        }

        return left;
    }

    private Node ParseFactor()
    {
        var left = ParsePrimary();
        
        while (CurrentToken.Type is TokenType.MULTIPLICATION or TokenType.DIVISION or TokenType.MODULUS)
        {   
            if (CurrentToken.Type is TokenType.MULTIPLICATION)
            {
                MoveNext();
                var node = new MultiplicationNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParsePrimary();
                left = node;
            }
            else if (CurrentToken.Type is TokenType.DIVISION)
            {
                MoveNext();
                var node = new DivisionNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParsePrimary();
                left = node;
            }
            else if (CurrentToken.Type is TokenType.MODULUS)
            {
                MoveNext();
                var node = new ModulusNode();
                node.Value.LeftChild = left;
                node.Value.RightChild = ParsePrimary();
                left = node;
            }
        }

        return left;
    }
    
    private Node ParsePrimary()
    {
        Node result;
        
        if (CurrentToken.Type == TokenType.INTEGER)
        {
            result = new IntegerNode(int.Parse(CurrentToken.Value));
        }
        else if (CurrentToken.Type == TokenType.DECIMAL)
        {
            result = new DecimalNode(decimal.Parse(CurrentToken.Value));
        }
        else if (CurrentToken.Type == TokenType.BOOLEAN)
        {
            result = new BooleanNode(bool.Parse(CurrentToken.Value));
        }
        else if (CurrentToken.Type == TokenType.STRING)
        {
            result = new StringNode(CurrentToken.Value);
        }
        else if (CurrentToken.Type == TokenType.IDENTIFIER)
        {
            result = new IdentifierNode(CurrentToken.Value);
        }
        else if (CurrentToken.Type == TokenType.PARAN_OPEN)
        {
            MoveNext();
            
            var expression = ParseExpression();

            if (CurrentToken.Type != TokenType.PARAN_CLOSE)
            {
                Diagnostics.LogError(CurrentToken.Meta, "Expected a close parenthesis.");
            }
            
            MoveNext();

            return expression;
        }
        else
        {
            Diagnostics.LogError(CurrentToken.Meta, "Invalid primary.");
            result = new ErrorNode();
        }
        
        MoveNext();
        return result;
    }
}