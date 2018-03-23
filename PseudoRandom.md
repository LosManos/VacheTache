# PseudoRandom

## Constructors

The default constructor has a optional string parameter used as randomising seed.  
If not provided [`[CallerMemberName]`](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute?view=netcore-1.1) is used.  
This makes it easy to have deterministic randomsing so any test run with a randomised value can be repeated exactly.

	 PseudoRandom([CallerMemberName] string seed = null);
	 PseudoRandom(Func<string, int> hashCodeFunc, string seed);

### Examples
The use of `[CallerMemberName]` makes it easy to set it up in a test method like so:  

	[TestMethod]
		public void MyTest()
		{
		// Arrange.
		var pr = new PseudoRandom();
		var value = pr.PositiveInt();
		...
	}

The pr is seeded with the name of the test method, "TestMethod", and although `value` contains a randomised value, every run of the test will contain the same value in `value`. (as long as the test method keeps its name)

If you prefer as an instance variable, use it like so:

	[TestClass]
	public class MyTests
	{
		private PseudoRandom _pr;

		// Since this property exists it is set by magic by the MsTest testing framework.
		public TestContext TestContext { get; set; }

		// This method is run before every test so every test gets its own seed.
		[TestInitialize]
		public void Setup()
		{
			_pr = new PseudoRandom(TestContext.TestName);
		}

		[TestMethod]
		public void MyTest()
		{
			var value = pr.PositiveInt();
			...
		}
	}

### Methods

See `VacheTache` for methods as `Pseudorandom` is just a way to make the randomising deterministic..
