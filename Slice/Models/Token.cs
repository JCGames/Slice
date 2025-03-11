namespace Slice.Models;

public enum TokenType
{
    // BASIC
    END_OF_FILE,
    END_OF_LINE,
    SINGLE_LINE_COMMENT,
    KEYWORD,
    TYPE_KEYWORD,
    IDENTIFIER,
    STRING,
    BOOLEAN,
    INTEGER,
    DECIMAL,
    RETURN,
    
    // BLOCKS
    BLOCK_OPEN,
    BLOCK_CLOSE,
    PARAN_OPEN,
    PARAN_CLOSE,
    SQUARE_OPEN,
    SQUARE_CLOSE,
    
    // SINGLE CHARACTERS
    GREATER_THAN,
    LESS_THAN,
    DOT,
    ADDITION,
    SUBTRACTION,
    MULTIPLICATION,
    DIVISION,
    MODULUS,
    COLON,
    COMMA,
    
    // DOUBLE CHARACTERS
    ASSIGNMENT,
    EQUALS,
    NOT_EQUALS,
    GREATER_THAN_OR_EQUAL,
    LESS_THAN_OR_EQUAL,
    AND,
    OR,
    // BITWISE_OR,
    // BITWISE_AND,
    // BITWISE_XOR,
    // BITWISE_LEFT_SHIFT,
    // BITWISE_RIGHT_SHIFT,
    // BITWISE_NOT
}

public class Token(
    TokenType type,
    string value,
    Meta meta)
{
    public readonly TokenType Type = type;
    public readonly string Value = value;
    public readonly Meta Meta = meta;

    public override string ToString()
    {
        return $"{Type}: |{Value}|, Line: {Meta.Line}, {{ {Meta.Start} -> {Meta.End} }}";
    }
}