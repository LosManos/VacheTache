using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using VacheTacheLibrary;

namespace FileSystemHelperTest
{
    public abstract class EntityTestBase
    {
        public TestContext TestContext { get; set; }

        protected PseudoRandom _pr;

        protected string ThePath => Path.Combine(Directory.GetCurrentDirectory(),
            TestContext.TestName);

        [TestInitialize]
        public void TestInitialize()
        {
            //  Setup the deterministic random values.
            _pr = new PseudoRandom(TestContext.TestName);
        }
    }
}
