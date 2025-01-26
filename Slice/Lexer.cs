using System.Collections;
using Slice.Models;

namespace Slice;

public class Lexer : IEnumerable<Token>
{
    private Scanner _scanner = null!;
    private readonly List<Token> _tokens = [];
    private long _currentLine = 1;

    private const string Assignment = "<-";
    private new const string Equals = "==";
    private const string NotEquals = "!=";
    private const string GreaterThan = ">";
    private const string LessThan = "<";
    private const string GreaterThanOrEqual = ">=";
    private const string LessThanOrEqual = "<=";
    private const string DotAccessor = ".";
    private const string BlockOpen = "{";
    private const string BlockClose = "}";
    private const string ParanOpen = "(";
    private const string ParanClose = ")";
    private const string SquareOpen = "[";
    private const string SquareClose = "]";
    
    public static Lexer FromFile(string filePath) => new()
    {
        _scanner = Scanner.FromFile(filePath)
    };
    
    public static Lexer FromText(string fileName, string text) => new()
    {
        _scanner = Scanner.FromText(fileName, text)
    };

    public List<Token> Tokenize()
    {
        while (!_scanner.IsEndOfStream) ScanNextToken();
        
        _tokens.Add(new Token(
            TokenType.END_OF_FILE,
            string.Empty,
            _currentLine,
            _scanner.Index,
            _scanner.Index));
        
        return _tokens;
    }

    private void ScanNextToken()
    {
        // END OF LINE
        if (_scanner.Current is ';')
        {
            _tokens.Add(new Token(
                TokenType.END_OF_LINE, 
                string.Empty, 
                _currentLine, 
                _scanner.Index,
                _scanner.Index));

            _scanner.Next();
        }
        else if (_scanner.IsEndOfLine)
        {
            _tokens.Add(new Token(
                TokenType.END_OF_LINE, 
                string.Empty, 
                _currentLine, 
                _scanner.Index,
                _scanner.Index));

            _currentLine++;

            if (Scanner.IsEndOfLineCharacter(_scanner.Peek())) _scanner.Next();
            _scanner.Next();
        }
        // SINGLE LINE COMMENT
        else if (_scanner.Current is '#')
        {
            ReadSingleLineComment();
        }
        // IDENTIFIER
        else if (_scanner.Current is '_' || _scanner.IsLetter)
        {
            ReadIdentifier();
        }
        // NUMBER
        else if (_scanner.IsDigit || (_scanner.Current is '.' && char.IsDigit(_scanner.Peek())))
        {
            ReadNumber();
        }
        // STRING
        else if (_scanner.Current is '"')
        {
            ReadString();
        }
        
        //  ================
        //  DOUBLE OPERATORS
        //  ================
        
        // ASSIGNMENT
        else if (_scanner.Current is '<' && _scanner.Peek() is '-')
        {
            _tokens.Add(new Token(
                TokenType.ASSIGNMENT,
                Assignment,
                _currentLine,
                _scanner.Index,
                _scanner.Index + 1));
            
            _scanner.Next();
            _scanner.Next();
        }
        // EQUALS
        else if (_scanner.Current is '=' && _scanner.Peek() is '=')
        {
            _tokens.Add(new Token(
                TokenType.EQUALS,
                Equals,
                _currentLine,
                _scanner.Index,
                _scanner.Index + 1));
            
            _scanner.Next();
            _scanner.Next();
        }
        // NOT EQUALS
        else if (_scanner.Current is '!' && _scanner.Peek() is '=')
        {
            _tokens.Add(new Token(
                TokenType.NOT_EQUALS,
                NotEquals,
                _currentLine,
                _scanner.Index,
                _scanner.Index + 1));
            
            _scanner.Next();
            _scanner.Next();
        }
        // GREATER THAN OR EQUAL
        else if (_scanner.Current is '>' && _scanner.Peek() is '=')
        {
            _tokens.Add(new Token(
                TokenType.GREATER_THAN_OR_EQUAL,
                GreaterThanOrEqual,
                _currentLine,
                _scanner.Index,
                _scanner.Index + 1));
            
            _scanner.Next();
            _scanner.Next();
        }
        // LESS THAN OR EQUAL
        else if (_scanner.Current is '<' && _scanner.Peek() is '=')
        {
            _tokens.Add(new Token(
                TokenType.LESS_THAN_OR_EQUAL,
                LessThanOrEqual,
                _currentLine,
                _scanner.Index,
                _scanner.Index + 1));
            
            _scanner.Next();
            _scanner.Next();
        }
        
        //  ================
        //  SINGLE OPERATORS
        //  ================
        
        // GREATER THAN
        else if (_scanner.Current is '>')
        {
            _tokens.Add(new Token(
                TokenType.GREATER_THAN,
                GreaterThan,
                _currentLine,
                _scanner.Index,
                _scanner.Index));
            
            _scanner.Next();
        }
        // LESS THAN
        else if (_scanner.Current is '<')
        {
            _tokens.Add(new Token(
                TokenType.LESS_THAN,
                LessThan,
                _currentLine,
                _scanner.Index,
                _scanner.Index));
            
            _scanner.Next();
        }
        // DOT ACCESSOR
        else if (_scanner.Current is '.')
        {
            _tokens.Add(new Token(
                TokenType.DOT_ACCESSOR,
                DotAccessor,
                _currentLine,
                _scanner.Index,
                _scanner.Index));
            
            _scanner.Next();
        }
        // BLOCK OPEN
        else if (_scanner.Current is '{')
        {
            _tokens.Add(new Token(
                TokenType.BLOCK_OPEN,
                BlockOpen,
                _currentLine,
                _scanner.Index,
                _scanner.Index));
            
            _scanner.Next();
        }
        // BLOCK CLOSE
        else if (_scanner.Current is '}')
        {
            _tokens.Add(new Token(
                TokenType.BLOCK_CLOSE,
                BlockClose,
                _currentLine,
                _scanner.Index,
                _scanner.Index));
            
            _scanner.Next();
        }
        // PARAN OPEN
        else if (_scanner.Current is '(')
        {
            _tokens.Add(new Token(
                TokenType.PARAN_OPEN,
                ParanOpen,
                _currentLine,
                _scanner.Index,
                _scanner.Index));
            
            _scanner.Next();
        }
        // PARAN CLOSE
        else if (_scanner.Current is ')')
        {
            _tokens.Add(new Token(
                TokenType.PARAN_CLOSE,
                ParanClose,
                _currentLine,
                _scanner.Index,
                _scanner.Index));
            
            _scanner.Next();
        }
        // SQUARE OPEN
        else if (_scanner.Current is '[')
        {
            _tokens.Add(new Token(
                TokenType.SQUARE_OPEN,
                SquareOpen,
                _currentLine,
                _scanner.Index,
                _scanner.Index));
            
            _scanner.Next();
        }
        // SQUARE CLOSE
        else if (_scanner.Current is ']')
        {
            _tokens.Add(new Token(
                TokenType.SQUARE_CLOSE,
                SquareClose,
                _currentLine,
                _scanner.Index,
                _scanner.Index));
            
            _scanner.Next();
        }
        else
        {
            _scanner.Next();
        }
    }

    private void ReadString()
    {
        var str = string.Empty;
        var startIndex = _scanner.Index + 1;
        
        _scanner.Next();

        while (!_scanner.IsEndOfStream)
        {
            str += _scanner.Current;

            if (_scanner.Peek() is '"') break;
            
            _scanner.Next();
        }
        
        _tokens.Add(new Token(
            TokenType.STRING,
            str, 
            _currentLine, 
            startIndex, 
            _scanner.Index));
        
        _scanner.Next();
        _scanner.Next();
    }

    private void ReadSingleLineComment()
    {
        var comment = string.Empty;
        var startIndex = _scanner.Index;
        
        _scanner.Next();

        while (!_scanner.IsEndOfStream)
        {
            comment += _scanner.Current;

            if (Scanner.IsEndOfLineCharacter(_scanner.Peek())) break;
            
            _scanner.Next();
        }
        
        _tokens.Add(new Token(
            TokenType.SINGLE_LINE_COMMENT,
            comment, 
            _currentLine, 
            startIndex, 
            _scanner.Index));
        
        _scanner.Next();
    }

    private void ReadIdentifier()
    {
        var identifier = string.Empty;
        var startIndex = _scanner.Index;

        while (!_scanner.IsEndOfStream)
        {
            identifier += _scanner.Current;

            if (_scanner.Peek() is not '_' && !char.IsLetterOrDigit(_scanner.Peek())) break;

            _scanner.Next();
        }

        var type = identifier switch
        {
            "true" or "false" or "True" or "False" => TokenType.BOOLEAN,
            "int" or "decimal" => TokenType.TYPE,
            "while" or "if" or "else" => TokenType.KEYWORD,
            _ => TokenType.IDENTIFIER
        };
        
        _tokens.Add(new Token(
            type,
            identifier,
            _currentLine,
            startIndex,
            _scanner.Index));

        _scanner.Next();
    }

    private void ReadNumber()
    {
        var number = string.Empty;
        var startIndex = _scanner.Index;
        var isDecimal = false;

        while (!_scanner.IsEndOfStream)
        {
            number += _scanner.Current;
            
            if (_scanner.Current is '.') isDecimal = true;
            if (_scanner.Peek() is not '.' && !char.IsDigit(_scanner.Peek())) break;
            
            _scanner.Next();
        }

        _tokens.Add(new Token(
            isDecimal ? TokenType.DECIMAL : TokenType.INTEGER,
            number,
            _currentLine,
            startIndex,
            _scanner.Index));
        
        _scanner.Next();
    }
    
    public IEnumerator<Token> GetEnumerator()
    {
        return _tokens.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _tokens.GetEnumerator();
    }
}