# KaosKo

## KaosKo library

KaosKo is a library for randomising different types of data.

## PseudoRandom library

**TBA**  
PseudoRandom has a slightly different raison d'ètre than KaosKo.
Namely to *not* generate randomised data but determistic data. This is for instance used for automatic tests where one wants to know the input and hence output.  
It is really just a descendant of KaosKo with a nifty constructor and a name that reminds of its purpose.

## Constructors

### KaosKo
The default constructor has an optional long parameter.  
It not provided DateTime.UtcNow.Ticks is used.  
This makes it easy to create the class and have randomised values.

### PseudoRandom
The default constructor has a optional string parameter.  
If not provided [[CallerMemberName]](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute?view=netcore-1.1) is used. This makes it easy to set it up in a test method like so:  

    [TestMethod]
    public void MyTest()
    {
        // Arrange.
        var pr = new PseudoRandom();
        var value = pr.PositiveInt();
        ...
    }

or if you prefer as an instance variable like so:

    [TestClass]
    public class MyTests
    {
        private PseudoRandom pr;

        // Since this property exists it is set by magic by the Mstest testing framework.
        public TestContext TestContext { get; set; }

        // This method is run before every test so every test gets its own seed.
        [TestInitialize]
        public void Setup()
        {
            pr = new PseudoRandom(TestContext.TestName);
        }

        [TestMethod]
        public void MyTest()
        {
            var value = pr.PositiveInt();
            ...
        }
    }


## Methods

### Bool
This method resturns a randomised [boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean) value.

### Date
This method returns a [datetime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime) with time set to midnight.  
There are overloaded methods creating a date in an interval.

### DateAndTime
This method returns a [datetime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime) where time is hours, minutes and seconds. Anything finer grained than a second is 0.

### Decimal
This method returns a (C#) [decimal](https://docs.microsoft.com/en-us/dotnet/articles/csharp/language-reference/keywords/decimal).  
There is *not* an overloaded method for creating a decimal in an interval.  
The result is probably not evenly distributed.

### Guid
This method returns a [GUID](https://docs.microsoft.com/en-us/dotnet/api/system.guid).  
Note that the value might not be valid GUID according to the [5 variants](https://en.wikipedia.org/wiki/Universally_unique_identifier#Versions) there are.  
Dotnet and Sqlserver recognises it though because 128bits are 128 bits.

### Int
This method returns a [32 bit signed integer](https://docs.microsoft.com/en-us/dotnet/api/system.int32).  
There is an overloaded method creating an integer in an interval.

### PositiveInt
This method returns a 32 bit positive [integer](https://docs.microsoft.com/en-us/dotnet/api/system.int32).  
0 is *not* considered a positive number.  
There are overloaded methods creating a positive integer in an interval.

### PositiveLong
This method returns a 64 bit positive [long](https://docs.microsoft.com/en-us/dotnet/api/system.int64).  
0 is *not* considred a positive number.  
There are overloaded methods creating a positive long in an interval.