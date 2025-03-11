using Slice;
using Slice.Parser;

namespace Tests;

[TestClass]
public class ParserTests
{
    [TestMethod]
    public void TestParser()
    {
        Diagnostics.ThrowInsteadOfExiting();
        
        const string code = "50 + 32.6 * 4";
        
        var rootNode = Parser
            .FromText("test", code)
            .Parse();

        var root = Parser.Analyze(rootNode);
        
        root.Print(string.Empty);
    }
    
    [TestMethod]
    public void TestFileParse()
    {
        Diagnostics.ThrowInsteadOfExiting();
        
        var root = Parser
            .FromFile("empty.slice")
            .Parse();
        
        var analyzedTree = Parser.Analyze(root);
        analyzedTree.Print(string.Empty);
    }
}