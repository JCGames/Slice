using Slice.Models;
using Slice;

namespace Tests;

[TestClass]
public class LexerTests
{
    [TestMethod]
    public void TestLexer()
    {
        const string code = """
                            # this is a comment
                            int decimal while if else true false True False
                            my_identifier _my_variable _myValue1to2
                            "This is a string"
                            23 23.5 .5
                            <- == != >= <= > < . { } ( ) [ ]
                            + - * / %
                            """;

        var tokens = Lexer
            .FromText("test", code)
            .Tokenize();

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }
        
        Assert.AreEqual(TokenType.SINGLE_LINE_COMMENT, tokens[0].Type);
        Assert.AreEqual(" this is a comment", tokens[0].Value);
        
        Assert.AreEqual(TokenType.END_OF_LINE, tokens[1].Type);
        
        Assert.AreEqual(TokenType.TYPE, tokens[2].Type);
        Assert.AreEqual("int", tokens[2].Value);

        Assert.AreEqual(TokenType.TYPE, tokens[3].Type);
        Assert.AreEqual("decimal", tokens[3].Value);
        
        Assert.AreEqual(TokenType.KEYWORD, tokens[4].Type);
        Assert.AreEqual("while", tokens[4].Value);

        Assert.AreEqual(TokenType.KEYWORD, tokens[5].Type);
        Assert.AreEqual("if", tokens[5].Value);
        
        Assert.AreEqual(TokenType.KEYWORD, tokens[6].Type);
        Assert.AreEqual("else", tokens[6].Value);

        Assert.AreEqual(TokenType.BOOLEAN, tokens[7].Type);
        Assert.AreEqual("true", tokens[7].Value);

        Assert.AreEqual(TokenType.BOOLEAN, tokens[8].Type);
        Assert.AreEqual("false", tokens[8].Value);
        
        Assert.AreEqual(TokenType.BOOLEAN, tokens[9].Type);
        Assert.AreEqual("True", tokens[9].Value);
        
        Assert.AreEqual(TokenType.BOOLEAN, tokens[10].Type);
        Assert.AreEqual("False", tokens[10].Value);
        
        Assert.AreEqual(TokenType.END_OF_LINE, tokens[11].Type);

        Assert.AreEqual(TokenType.IDENTIFIER, tokens[12].Type);
        Assert.AreEqual("my_identifier", tokens[12].Value);
        
        Assert.AreEqual(TokenType.IDENTIFIER, tokens[13].Type);
        Assert.AreEqual("_my_variable", tokens[13].Value);
        
        Assert.AreEqual(TokenType.IDENTIFIER, tokens[14].Type);
        Assert.AreEqual("_myValue1to2", tokens[14].Value);
        
        Assert.AreEqual(TokenType.END_OF_LINE, tokens[15].Type);

        Assert.AreEqual(TokenType.STRING, tokens[16].Type);
        Assert.AreEqual("This is a string", tokens[16].Value);
        
        Assert.AreEqual(TokenType.END_OF_LINE, tokens[17].Type);

        Assert.AreEqual(TokenType.INTEGER, tokens[18].Type);
        Assert.AreEqual("23", tokens[18].Value);
        
        Assert.AreEqual(TokenType.DECIMAL, tokens[19].Type);
        Assert.AreEqual("23.5", tokens[19].Value);
        
        Assert.AreEqual(TokenType.DECIMAL, tokens[20].Type);
        Assert.AreEqual(".5", tokens[20].Value);
        
        Assert.AreEqual(TokenType.END_OF_LINE, tokens[21].Type);

        Assert.AreEqual(TokenType.ASSIGNMENT, tokens[22].Type);
        Assert.AreEqual(TokenType.EQUALS, tokens[23].Type);
        Assert.AreEqual(TokenType.NOT_EQUALS, tokens[24].Type);
        Assert.AreEqual(TokenType.GREATER_THAN_OR_EQUAL, tokens[25].Type);
        Assert.AreEqual(TokenType.LESS_THAN_OR_EQUAL, tokens[26].Type);
        Assert.AreEqual(TokenType.GREATER_THAN, tokens[27].Type);
        Assert.AreEqual(TokenType.LESS_THAN, tokens[28].Type);
        Assert.AreEqual(TokenType.DOT_ACCESSOR, tokens[29].Type);
        Assert.AreEqual(TokenType.BLOCK_OPEN, tokens[30].Type);
        Assert.AreEqual(TokenType.BLOCK_CLOSE, tokens[31].Type);
        Assert.AreEqual(TokenType.PARAN_OPEN, tokens[32].Type);
        Assert.AreEqual(TokenType.PARAN_CLOSE, tokens[33].Type);
        Assert.AreEqual(TokenType.SQUARE_OPEN, tokens[34].Type);
        Assert.AreEqual(TokenType.SQUARE_CLOSE, tokens[35].Type);
        
        Assert.AreEqual(TokenType.END_OF_LINE, tokens[36].Type);
        
        Assert.AreEqual(TokenType.ADDITION, tokens[37].Type);
        Assert.AreEqual(TokenType.SUBTRACTION, tokens[38].Type);
        Assert.AreEqual(TokenType.MULTIPLICATION, tokens[39].Type);
        Assert.AreEqual(TokenType.DIVISION, tokens[40].Type);
        Assert.AreEqual(TokenType.MODULUS, tokens[41].Type);

        Assert.AreEqual(TokenType.END_OF_FILE, tokens[42].Type);
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
}