namespace Slice.Models;

public enum TokenType
{
    END_OF_FILE,
    END_OF_LINE,
    SINGLE_LINE_COMMENT,
    TYPE,
    KEYWORD,
    IDENTIFIER,
    STRING,
    BOOLEAN,
    INTEGER,
    DECIMAL,
    DOT_ACCESSOR,
    BLOCK_OPEN,
    BLOCK_CLOSE,
    PARAN_OPEN,
    PARAN_CLOSE,
    SQUARE_OPEN,
    SQUARE_CLOSE,
    ASSIGNMENT,
    EQUALS,
    NOT_EQUALS,
    GREATER_THAN,
    LESS_THAN,
    GREATER_THAN_OR_EQUAL,
    LESS_THAN_OR_EQUAL,
    BITWISE_OR,
    BITWISE_AND,
    BITWISE_XOR,
    BITWISE_LEFT_SHIFT,
    BITWISE_RIGHT_SHIFT,
    BITWISE_NOT
}

public class Token(
    TokenType type,
    string value,
    long line,
    long start,
    long end)
{
    public readonly TokenType Type = type;
    public readonly string Value = value;

    public readonly Context Context = new()
    {
        Line = line,
        Start = start,
        End = end
    };

    public override string ToString()
    {
        return $"{Type}: |{Value}|, Line: {Context.Line}, {{ {Context.Start} -> {Context.End} }}";
    }
}