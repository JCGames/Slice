using Slice;

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
        
        rootNode?.Print(string.Empty);
    }
}