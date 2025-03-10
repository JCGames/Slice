using System.Collections.Immutable;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using Slice.Models;
using Slice.Models.Nodes;
using Slice.Models.Nodes.BinaryOperators;
using Slice.Models.Nodes.ValueNodes;

namespace Slice.Parser;

public sealed partial class Parser
{
    private ImmutableList<Token> _tokens = [];
    private int _currentIndex;
    private Token CurrentToken => _tokens[_currentIndex];

    private readonly Token _errorToken = new(TokenType.END_OF_FILE, "ERROR", new Meta("", 0, 0, 0));

    private bool HasNext => _currentIndex < _tokens.Count - 1;
    
    private void MoveNext()
    {
        if (_currentIndex >= _tokens.Count) return;
        
        _currentIndex++;
    }

    private Token PeekNext()
    {
        if (_currentIndex + 1 >= _tokens.Count) return _errorToken;
        
        return _tokens[_currentIndex + 1];
    }

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

    public BlockNode Parse()
    {
        return ParseBlock(ParseBlockOption.None);
    }
}