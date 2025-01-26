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
    private bool _hasNext = true;
    
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
        if (_currentIndex + 1 >= _tokens.Count)
        {
            _hasNext = false;
            return;
        }

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
        _hasNext = true;
    }

    private static void AssertType(TokenType expectedType, Token token)
    {
        if (token.Type == expectedType) return;
        Diagnostics.LogError(0, 0, 0, $"Expected a {expectedType}, but found {token.Type}");
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

    private Expression ParseExpression() => new()
    {
        Value = ParseAdditionAndSubtraction()
    };

    private Node? ParseAdditionAndSubtraction()
    {
        var result = ParseMultiplicationAndDivision();

        while (_currentToken.Type is TokenType.ADDITION or TokenType.SUBTRACTION)
        {
            if (_currentToken.Type is TokenType.ADDITION)
            {
                Next();
                var addition = new AdditionNode();
                addition.Value.LeftChild = result;
                addition.Value.RightChild = ParseMultiplicationAndDivision();
                result = addition;
            }
            else if (_currentToken.Type is TokenType.SUBTRACTION)
            {
                Next();
                var subtraction = new SubtractionNode();
                subtraction.Value.LeftChild = result;
                subtraction.Value.RightChild = ParseMultiplicationAndDivision();
                result = subtraction;
            }
        }

        return result;
    }

    private Node? ParseMultiplicationAndDivision()
    {
        var result = ParseTerm();

        while (_currentToken.Type is TokenType.MULTIPLICATION or TokenType.DIVISION or TokenType.MODULUS)
        {
            if (_currentToken.Type is TokenType.MULTIPLICATION)
            {
                Next();
                var multiplication = new MultiplicationNode();
                multiplication.Value.LeftChild = result;
                multiplication.Value.RightChild = ParseTerm();
                result = multiplication;
            }
            else if (_currentToken.Type is TokenType.DIVISION)
            {
                Next();
                var division = new DivisionNode();
                division.Value.LeftChild = result;
                division.Value.RightChild = ParseTerm();
                result = division;
            }
            else if (_currentToken.Type is TokenType.MODULUS)
            {
                Next();
                var modulus = new ModulusNode();
                modulus.Value.LeftChild = result;
                modulus.Value.RightChild = ParseTerm();
                result = modulus;
            }
        }

        return result;
    }

    private Node? ParseTerm()
    {   
        Node? result = _currentToken.Type switch
        {
            TokenType.INTEGER => new IntegerNode(int.Parse(_currentToken.Value)),
            TokenType.DECIMAL => new DecimalNode(double.Parse(_currentToken.Value)),
            TokenType.STRING => new StringNode(_currentToken.Value),
            TokenType.IDENTIFIER => new IdentifierNode(_currentToken.Value),
            TokenType.BOOLEAN => new BooleanNode(bool.Parse(_currentToken.Value)),
            _ => null
        };
        
        Next();

        return result;
    }
}