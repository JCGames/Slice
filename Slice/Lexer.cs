using System.Collections;
using Slice.Models;

namespace Slice;

public sealed class Lexer
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
    private const string Addition = "+";
    private const string Subtraction = "-";
    private const string Multiplication = "*";
    private const string Division = "/";
    private const string Modulus = "%";
    
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
            new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
        
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
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));

            _scanner.Next();
        }
        else if (_scanner.IsEndOfLine)
        {
            _tokens.Add(new Token(
                TokenType.END_OF_LINE, 
                string.Empty, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));

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
        //  BINARY OPERATORS
        //  ================
        
        // ASSIGNMENT
        else if (_scanner.Current is '<' && _scanner.Peek() is '-')
        {
            _tokens.Add(new Token(
                TokenType.ASSIGNMENT, 
                Assignment, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index + 1)));
            
            _scanner.Next();
            _scanner.Next();
        }
        // EQUALS
        else if (_scanner.Current is '=' && _scanner.Peek() is '=')
        {
            _tokens.Add(new Token(
                TokenType.EQUALS, 
                Equals, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index + 1)));
            
            _scanner.Next();
            _scanner.Next();
        }
        // NOT EQUALS
        else if (_scanner.Current is '!' && _scanner.Peek() is '=')
        {
            _tokens.Add(new Token(
                TokenType.NOT_EQUALS, 
                NotEquals, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index + 1)));
            
            _scanner.Next();
            _scanner.Next();
        }
        // GREATER THAN OR EQUAL
        else if (_scanner.Current is '>' && _scanner.Peek() is '=')
        {
            _tokens.Add(new Token(
                TokenType.GREATER_THAN_OR_EQUAL, 
                GreaterThanOrEqual, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index + 1)));
            
            _scanner.Next();
            _scanner.Next();
        }
        // LESS THAN OR EQUAL
        else if (_scanner.Current is '<' && _scanner.Peek() is '=')
        {
            _tokens.Add(new Token(
                TokenType.LESS_THAN_OR_EQUAL, 
                LessThanOrEqual, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index + 1)));
            
            _scanner.Next();
            _scanner.Next();
        }
        
        //  ===============
        //  UNARY OPERATORS
        //  ===============
        
        // GREATER THAN
        else if (_scanner.Current is '>')
        {
            _tokens.Add(new Token(
                TokenType.GREATER_THAN, 
                GreaterThan, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        // LESS THAN
        else if (_scanner.Current is '<')
        {
            _tokens.Add(new Token(
                TokenType.LESS_THAN, 
                LessThan, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        // DOT ACCESSOR
        else if (_scanner.Current is '.')
        {
            _tokens.Add(new Token(
                TokenType.DOT_ACCESSOR, 
                DotAccessor,
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        // ADDITION
        else if (_scanner.Current is '+')
        {
            _tokens.Add(new Token(
                TokenType.ADDITION, 
                Addition, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        // SUBTRACTION
        else if (_scanner.Current is '-')
        {
            _tokens.Add(new Token(
                TokenType.SUBTRACTION, 
                Subtraction, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        // MULTIPLICATION
        else if (_scanner.Current is '*')
        {
            _tokens.Add(new Token(
                TokenType.MULTIPLICATION, 
                Multiplication, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        // DIVISION
        else if (_scanner.Current is '/')
        {
            _tokens.Add(new Token(
                TokenType.DIVISION, 
                Division, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        // MODULUS
        else if (_scanner.Current is '%')
        {
            _tokens.Add(new Token(
                TokenType.MODULUS, 
                Modulus, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        
        // ======
        // BLOCKS
        // ======
        
        // BLOCK OPEN
        else if (_scanner.Current is '{')
        {
            _tokens.Add(new Token(
                TokenType.BLOCK_OPEN, 
                BlockOpen, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        // BLOCK CLOSE
        else if (_scanner.Current is '}')
        {
            _tokens.Add(new Token(
                TokenType.BLOCK_CLOSE, 
                BlockClose, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        // PARAN OPEN
        else if (_scanner.Current is '(')
        {
            _tokens.Add(new Token(
                TokenType.PARAN_OPEN, 
                ParanOpen, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        // PARAN CLOSE
        else if (_scanner.Current is ')')
        {
            _tokens.Add(new Token(
                TokenType.PARAN_CLOSE, 
                ParanClose, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        // SQUARE OPEN
        else if (_scanner.Current is '[')
        {
            _tokens.Add(new Token(
                TokenType.SQUARE_OPEN, 
                SquareOpen, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        // SQUARE CLOSE
        else if (_scanner.Current is ']')
        {
            _tokens.Add(new Token(
                TokenType.SQUARE_CLOSE, 
                SquareClose, 
                new Meta(_scanner.FilePath, _currentLine, _scanner.Index, _scanner.Index)));
            
            _scanner.Next();
        }
        else
        {
            if (!_scanner.IsWhitespace) Diagnostics.LogError(_currentLine, _scanner.Index, _scanner.Index, "Found unknown character.");

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
            new Meta(_scanner.FilePath, _currentLine, startIndex, _scanner.Index)));
        
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
            new Meta(_scanner.FilePath, _currentLine, startIndex, _scanner.Index)));
        
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
            new Meta(_scanner.FilePath, _currentLine, startIndex, _scanner.Index)));

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

            if (_scanner.Current is '.')
            {
                if (!isDecimal)
                {
                    isDecimal = true;
                }
                else
                {
                    Diagnostics.LogError(_currentLine, startIndex, _scanner.Index, $"To many decimal points.");
                }
            }
            
            if (_scanner.Peek() is not '.' && !char.IsDigit(_scanner.Peek())) break;
            
            _scanner.Next();
        }

        _tokens.Add(new Token(
            isDecimal ? TokenType.DECIMAL : TokenType.INTEGER,
            number,
            new Meta(_scanner.FilePath, _currentLine, startIndex, _scanner.Index)));
        
        _scanner.Next();
    }
}