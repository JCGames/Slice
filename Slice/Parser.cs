using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Slice.Models;
using Slice.Models.Nodes;
using Slice.Models.Nodes.BinaryOperators;
using Slice.Models.Nodes.ValueNodes;

namespace Slice;

public sealed class Parser
{
    private List<Token> _tokens = [];
    private int _currentIndex;
    private Token _currentToken = null!;

    public static Parser FromFile(string filePath) => new()
    {
        _tokens = Lexer
            .FromFile(filePath)
            .Tokenize()
    };
    
    public static Parser FromText(string fileName, string text) => new()
    {
        _tokens = Lexer
            .FromText(fileName, text)
            .Tokenize()
    };

    private void Next()
    {
        if (_currentIndex + 1 >= _tokens.Count) return;

        _currentIndex++;
        _currentToken = _tokens[_currentIndex];
    }

    private bool TryPeek(out Token? token)
    {
        token = null;
        
        if (_currentIndex + 1 >= _tokens.Count) return false;
        
        token = _tokens[_currentIndex + 1];

        return true;
    }

    private void Back()
    {
        if (_currentIndex - 1 < 0) return;

        _currentIndex--;
        _currentToken = _tokens[_currentIndex];
    }

    private void AssertType(TokenType expectedType)
    {
        AssertType(expectedType, _currentToken);
    }

    private static void AssertType(TokenType expectedType, Token token)
    {
        if (token.Type == expectedType) return;
        Diagnostics.LogError(token.Meta, $"Expected a {expectedType}, but found {token.Type}");
    }
    
    public BlockNode? Parse()
    {
        if (_tokens.Count == 0) return null;
        _currentToken = _tokens[0];
        
        return ParseFile();
    }

    private BlockNode ParseFile()
    {
        var block = new BlockNode();
        
        while (_currentToken.Type is not TokenType.END_OF_FILE)
        {
            if (ParseNextNode() is { } node) block.Value.Add(node);
            else Next();
        }
        
        return block;
    }

    private Node? ParseNextNode()
    {
        return ParseExpression();
    }

    private Expression ParseExpression()
    {
        var node = ParseAssignment();

        if (_currentToken.Type is not TokenType.END_OF_LINE and not TokenType.END_OF_FILE)
        {
            Diagnostics.LogError(_currentToken.Meta, "Expression should end at new line");
        }

        return new Expression
        {
            Value = node
        };
    }

    private Node? ParseAssignment()
    {
        var nodes = new Stack<Node?>();
        nodes.Push(ParseTerm());
        Node? result = nodes.Peek();

        while (_currentToken.Type is TokenType.ASSIGNMENT)
        {
            Next();
            nodes.Push(ParseTerm());
        }

        if (nodes.Count > 2)
        {
            while (nodes.Count > 1)
            {
                var right = nodes.Pop();
                var left = nodes.Pop();

                AssertIsAssignable(left);

                result = new AssignmentNode
                {
                    Value = new LeftRightChild(left, right)
                };
            }

            AssertIsAssignable(nodes.Peek());

            result = new AssignmentNode
            {
                Value = new LeftRightChild(nodes.Pop(), result)
            };
        }
        else if (nodes.Count == 2)
        {
            AssertIsAssignable(result);

            result = new AssignmentNode
            {
                Value = new LeftRightChild(result, nodes.Pop())
            };
        }

        return result;

        void AssertIsAssignable(Node? node)
        {
            if (node is not IdentifierNode and not DeclarationNode or null)
            {
                Diagnostics.LogError(node?.Meta ?? _currentToken.Meta, "Invalid term to the left of the assignment operator.");
            }
        }
    }

    private Node? ParseTerm()
    {
        var result = ParseFactor();

        while (_currentToken.Type is TokenType.ADDITION or TokenType.SUBTRACTION)
        {
            if (_currentToken.Type is TokenType.ADDITION)
            {
                Next();
                result = new AdditionNode { Meta = _currentToken.Meta, Value = new LeftRightChild(result, ParseFactor()) };
            }
            else if (_currentToken.Type is TokenType.SUBTRACTION)
            {
                Next();
                result = new SubtractionNode { Meta = _currentToken.Meta, Value = new LeftRightChild(result, ParseFactor()) };
            }
        }

        return result;
    }

    private Node? ParseFactor()
    {
        var result = ParsePrimary();

        while (_currentToken.Type is TokenType.MULTIPLICATION or TokenType.DIVISION or TokenType.MODULUS)
        {
            if (_currentToken.Type is TokenType.MULTIPLICATION)
            {
                Next();
                result = new MultiplicationNode { Meta = _currentToken.Meta, Value = new LeftRightChild(result, ParseFactor()) };
            }
            else if (_currentToken.Type is TokenType.DIVISION)
            {
                Next();
                result = new DivisionNode { Meta = _currentToken.Meta, Value = new LeftRightChild(result, ParseFactor()) };
            }
            else if (_currentToken.Type is TokenType.MODULUS)
            {
                Next();
                result = new ModulusNode { Meta = _currentToken.Meta, Value = new LeftRightChild(result, ParseFactor()) };
            }
        }

        return result;
    }

    private Node? ParsePrimary()
    {   
        // allows functions defined in this methods
        // body to stop the default behaviour of
        // moving to the next token after parsing
        var shouldMoveToNext = true;

        Node? result = _currentToken.Type switch
        {
            TokenType.INTEGER => new IntegerNode(int.Parse(_currentToken.Value)),
            TokenType.DECIMAL => new DecimalNode(double.Parse(_currentToken.Value)),
            TokenType.STRING => new StringNode(_currentToken.Value),
            TokenType.IDENTIFIER => new IdentifierNode(_currentToken.Value),
            TokenType.BOOLEAN => new BooleanNode(bool.Parse(_currentToken.Value)),
            TokenType.PARAN_OPEN => GetNestedExpression(),
            TokenType.TYPE => GetVariableDeclaration(),
            _ => null
        };

        if (result is null) Diagnostics.LogError(_currentToken.Meta, $"Invalid primary \"{_currentToken.Value}\" in expression.");
        
        if (shouldMoveToNext)
        {
            Next();
        }

        return result;

        Expression GetNestedExpression()
        {
            Next();
            var exp = ParseExpression();

            AssertType(TokenType.PARAN_CLOSE);

            return exp;
        }

        DeclarationNode GetVariableDeclaration()
        {
            var node = new DeclarationNode();

            node.Value.LeftChild = new TypeNode(_currentToken.Value);

            Next();
            AssertType(TokenType.IDENTIFIER);

            node.Value.RightChild = new IdentifierNode(_currentToken.Value);

            Next();

            if (_currentToken.Type is not TokenType.END_OF_LINE and not TokenType.END_OF_FILE and not TokenType.ASSIGNMENT)
            {
                Diagnostics.LogError(_currentToken.Meta, $"Variable declarations can only be assigned to but found \"{_currentToken.Type}\".");
            }

            shouldMoveToNext = false;

            return node;
        }
    }
}