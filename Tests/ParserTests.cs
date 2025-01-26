using Slice;

namespace Tests;

[TestClass]
public class ParserTests
{
    [TestMethod]
    public void TestParser()
    {
        Diagnostics.DisableExiting();
        
        const string code = """
                            int number <- 10
                            int j
                            """;
        
        var rootNode = Parser
            .FromText("test", code)
            .Parse();
        
        rootNode?.Print(string.Empty);
    }
}