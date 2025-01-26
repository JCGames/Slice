using Slice.Models;
using Slice.Models.Nodes;

namespace Slice;

public class Parser
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

    private void AssertType(TokenType expectedType, Token token)
    {
        if (token.Type != expectedType)
        {
            Diagnostics.LogError($"Expected a {expectedType}, but found {token.Type}");
        }
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
            if (ParseNextNode() is { } node) block.Children.Add(node);
            else Next();
        }
        
        return block;
    }

    private BlockNode? ParseBlock()
    {
        return null;
    }

    private Node? ParseNextNode()
    {
        return _currentToken switch
        {
            { Type: TokenType.TYPE } => ParseVariableDeclarationNode(),
            _ => null
        };
    }

    /**
     * variable_declaration
     *      : type identifier assignment?
     *      ;
     */
    private Node ParseVariableDeclarationNode()
    {
        var node = new VariableDeclarationNode();
        node.Children.Add(new TypeNode(_currentToken.Value));
            
        Next();
        AssertType(TokenType.IDENTIFIER, _currentToken);
            
        node.Children.Add(new IdentifierNode(_currentToken.Value));
        return node;
    }

    /**
     * assignment
     *      : unary_expression assignment expression
     */
    private Node ParseAssignmentNode()
    {
        return new AssignmentNode();
    }
}