using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        
        public void TestAdd()
        {
            ClassLibrary1.Class1 p1 = new ClassLibrary1.Class1();

            Random rnd = new Random();
            int x = rnd.Next();
            int y = rnd.Next();

            Assert.AreEqual(p1.add(x, y), x + y);
        }
    }
}