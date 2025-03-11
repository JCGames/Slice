using System.Collections.Immutable;
using Slice.Models;
using Slice;

namespace Tests;

[TestClass]
public class LexerTests
{
    [TestMethod]
    public void TestLexer()
    {
        Diagnostics.ThrowInsteadOfExiting();
        
        var tokenTypes = Enum.GetNames(typeof(TokenType));

        foreach (var tokenType in tokenTypes)
        {
            if (_testTokens.All(x => x.ResultType.ToString() != tokenType))
            {
                Assert.Fail($"Missing a test for token type: {tokenType}.");
            }
        }
        
        foreach (var testToken in _testTokens)
        {
            var lexer = Lexer.FromText(string.Empty, testToken.Text);
            var tokens = lexer.Tokenize();

            if (tokens.Count > 1)
            {
                tokens = tokens.SkipLast(1).ToImmutableList();
            }
            
            if (tokens.Any(x => x.Type != testToken.ResultType))
            {
                Assert.Fail($"Not all results had a type of {testToken.ResultType}.");
            }
        }
    }

    [TestMethod]
    public void TestSingleCharacterAtEndOfStream()
    {
        const string code = "j";
        
        var tokens = Lexer
            .FromText("test", code)
            .Tokenize();

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    [TestMethod]
    public void TestDecimalOnlyHasOnePoint()
    {
        Diagnostics.ThrowInsteadOfExiting();

        const string code = "23.3.5";

        Assert.ThrowsException<DiagnosticsException>(() =>
        {
            var tokens = Lexer
                .FromText("test", code)
                .Tokenize();
        }, "Fatal Error <1:1-5>: To many decimal points.");
    }

    [TestMethod]
    public void TestTokensFromFile()
    {
        Diagnostics.ThrowInsteadOfExiting();

        var tokens = Lexer
            .FromFile("empty.slice")
            .Tokenize();
        
        tokens.ForEach(Console.WriteLine);
    }

    private class TestToken
    {
        public required string Text { get; set; }
        public required TokenType ResultType { get; set; }
    }

    private static readonly List<TestToken> _testTokens = [
        new()
        {
            Text = "\n",
            ResultType = TokenType.END_OF_LINE
        },
        new()
        {
            Text = "",
            ResultType = TokenType.END_OF_FILE
        },
        new()
        {
            Text = "#",
            ResultType = TokenType.SINGLE_LINE_COMMENT
        },
        new()
        {
            Text = "if loop else",
            ResultType = TokenType.KEYWORD
        },
        new()
        {
            Text = "Object",
            ResultType = TokenType.IDENTIFIER
        },
        new()
        {
            Text = "\"Some string\"",
            ResultType = TokenType.STRING
        },
        new()
        {
            Text = "true false",
            ResultType = TokenType.BOOLEAN
        },
        new()
        {
            Text = "10 32 44",
            ResultType = TokenType.INTEGER
        },
        new()
        {
            Text = "10.4 3.4 2.2",
            ResultType = TokenType.DECIMAL
        },
        new()
        {
            Text = "{",
            ResultType = TokenType.BLOCK_OPEN
        },
        new()
        {
            Text = "}",
            ResultType = TokenType.BLOCK_CLOSE
        },
        new()
        {
            Text = "(",
            ResultType = TokenType.PARAN_OPEN
        },
        new()
        {
            Text = ")",
            ResultType = TokenType.PARAN_CLOSE
        },
        new()
        {
            Text = "[",
            ResultType = TokenType.SQUARE_OPEN
        },
        new()
        {
            Text = "]",
            ResultType = TokenType.SQUARE_CLOSE
        },
        new()
        {
            Text = ">",
            ResultType = TokenType.GREATER_THAN
        },
        new()
        {
            Text = "<",
            ResultType = TokenType.LESS_THAN
        },
        new()
        {
            Text = ".",
            ResultType = TokenType.DOT
        },
        new()
        {
            Text = "+",
            ResultType = TokenType.ADDITION
        },
        new()
        {
            Text = "-",
            ResultType = TokenType.SUBTRACTION
        },
        new()
        {
            Text = "*",
            ResultType = TokenType.MULTIPLICATION
        },
        new()
        {
            Text = "/",
            ResultType = TokenType.DIVISION
        },
        new()
        {
            Text = "%",
            ResultType = TokenType.MODULUS
        },
        new()
        {
            Text = "<-",
            ResultType = TokenType.ASSIGNMENT
        },
        new()
        {
            Text = "true",
            ResultType = TokenType.BOOLEAN
        },
        new()
        {
            Text = "==",
            ResultType = TokenType.EQUALS
        },
        new()
        {
            Text = "!=",
            ResultType = TokenType.NOT_EQUALS
        },
        new()
        {
            Text = ">=",
            ResultType = TokenType.GREATER_THAN_OR_EQUAL
        },
        new()
        {
            Text = "<=",
            ResultType = TokenType.LESS_THAN_OR_EQUAL
        },
        new()
        {
            Text = "and",
            ResultType = TokenType.AND
        },
        new()
        {
            Text = "or",
            ResultType = TokenType.OR
        },
        new()
        {
            Text = "int decimal bool string",
            ResultType = TokenType.TYPE_KEYWORD
        },
        new()
        {
            Text = ":",
            ResultType = TokenType.COLON
        },
        new()
        {
            Text = ",",
            ResultType = TokenType.COMMA
        }
    ];
}