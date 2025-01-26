using Slice;

namespace Tests;

[TestClass]
public class ScannerTests
{
    [TestMethod]
    public void TestNext()
    {
        using var scanner = Scanner.FromFile("./Junk.txt");

        Assert.AreEqual('T', scanner.Current);
        scanner.Next();
        Assert.AreEqual('h', scanner.Current);
    }

    [TestMethod]
    public void TestPeek()
    {
        using var scanner = Scanner.FromFile("./Junk.txt");
        
        Assert.AreEqual('h', scanner.Peek());
        Assert.AreEqual('T', scanner.Current);
    }

    [TestMethod]
    public void TestBack()
    {
        using var scanner = Scanner.FromFile("./Junk.txt");

        scanner.Next();
        
        Assert.AreEqual('h', scanner.Current);
        scanner.Back();
        Assert.AreEqual('T', scanner.Current);

        scanner.Next();
        Assert.AreEqual('h', scanner.Current);
        
        scanner.Back();
        scanner.Back();
        
        Assert.AreEqual('T', scanner.Current);
    }
    
    [TestMethod]
    public void TestStringNext()
    {
        using var scanner = Scanner.FromText("test", "Wow");

        Assert.AreEqual('W', scanner.Current);
        scanner.Next();
        Assert.AreEqual('o', scanner.Current);
    }

    [TestMethod]
    public void TestStringPeek()
    {
        using var scanner = Scanner.FromText("test", "Wow");
        
        Assert.AreEqual('o', scanner.Peek());
        Assert.AreEqual('W', scanner.Current);
    }

    [TestMethod]
    public void TestStringBack()
    {
        using var scanner = Scanner.FromText("test", "Wow");

        scanner.Next();
        
        Assert.AreEqual('o', scanner.Current);
        scanner.Back();
        Assert.AreEqual('W', scanner.Current);

        scanner.Next();
        Assert.AreEqual('o', scanner.Current);
        
        scanner.Back();
        scanner.Back();
        
        Assert.AreEqual('W', scanner.Current);
    }
}