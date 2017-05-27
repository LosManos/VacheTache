# VacheTache

Version 1.0.0

This library is licensed under LGPLv3 license.  
This means that you are free to use this library in your (commercial) product as long as  you
1) attach the code for VacheTache
1) and any updates you have made to VacheTache
1) and the license for VacheTache  

to every customer of your (commercial) product.  
Your existing (commercial) product is not affected.

## VacheTache class

VacheTache is a class for randomising different types of data.

## PseudoRandom class

PseudoRandom has a slightly different raison d'ètre than VacheTache.
Namely to *not* generate randomised data but determistic data. This is for instance used for automatic tests where one wants to know the input and hence output.  
It is really just a descendant of VacheTache with a nifty constructor and a name that reminds of its purpose.

## Constructors

### VacheTache
The default constructor has an optional long parameter.  
It not provided DateTime.UtcNow.Ticks is used.  
This makes it easy to create the class and have randomised values.

	VacheTache(long? seed = null);
	VacheTache(Func<int,int> hashCodeFunc, long seed);

#### Examples

	var vt = new VacheTache();
	var address = vt.String("ADR-");
	// address is now something like "ADR-xdIC".

### PseudoRandom
The default constructor has a optional string parameter.  
If not provided [[CallerMemberName]](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.callermembernameattribute?view=netcore-1.1) is used. 

	 PseudoRandom([CallerMemberName] string seed = null);
	 PseudoRandom(Func<string, int> hashCodeFunc, string seed);

#### Examples
The use of [CallerMemberName] makes it easy to set it up in a test method like so:  

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

	bool Bool();

### Date
This method returns a [datetime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime) with time set to midnight.  
There are overloaded methods creating a date in an interval.

	DateTime Date();
	DateTime Date(DateTime from, DateTime to);

### DateAndTime
This method returns a [datetime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime) where time is hours, minutes and seconds. Anything finer grained than a second is 0.

	DateTime DateAndTime();
	DateTime DateAndTime(DateTime from, DateTime to);

### Decimal
This method returns a (C#) [decimal](https://docs.microsoft.com/en-us/dotnet/articles/csharp/language-reference/keywords/decimal).  
There is *not* an overloaded method for creating a decimal in an interval.  
The result is probably not evenly distributed.  
There are some worries around `Decimal(decimal minValue, decimal maxValue)` so it is not implemented.

	decimal Decimal();

### Currency
This method(s) returns a natural number or a natural number and 2 decimal fraction. The former is for use with currencies without decimals, i.e. yen. 
To set the number of decimals one either does that in the call or in the property `NumberOfCurrencyDecimals`.

	decimal Currency();
	decimal Currency(int min, int max);
	decimal Currency(int min, int max, int decimals);

**TBA**

Properties `CurrencyMin` and `CurrencyMax` for setting the min and max in a central, convenient place.

Generate currencies in even 5 or 10 intervals.

### Enum
This method returns an enum.

	TEnum> Enum<TEnum>();

Note that it, presently, does not handel inconsecutive value series or items with the same value.

### EnumExcept
This method returns an enum except the one provided in the parameter.

	TEnum EnumExcept<TEnum>(IEnumerable<TEnum> exceptEnums)where TEnum : struct

### Guid
This method returns a [GUID](https://docs.microsoft.com/en-us/dotnet/api/system.guid).  
Note that the value might not be valid GUID according to the [5 variants](https://en.wikipedia.org/wiki/Universally_unique_identifier#Versions) there are.  
Dotnet and Sqlserver recognises it though because 128bits are 128 bits.

	Guid Guid();

### Int
This method returns a [32 bit signed integer](https://docs.microsoft.com/en-us/dotnet/api/system.int32).  
There is an overloaded method creating an integer in an interval.

	int Int();
	int Int(int minValue, int maxValue);

### OneOf
This method returns an item of the list of items provided.

	T OneOf<T>(IEnumerable<T> lst)

#### Examples

	var pr = new VacheTache();
	var freedomBraves = pr.OneOf(new[]{"Assange", "Manning", "Hood", "Ghandi", "Mandela"});
	// freedomBraves becomes something like "Manning".

	var nr = pr.OneOf(new[]{3,14,15,9});
	// nr becomes something like 14.

### PositiveInt
This method returns a 32 bit positive [integer](https://docs.microsoft.com/en-us/dotnet/api/system.int32).  
0 is *not* considered a positive number.  
There are overloaded methods creating a positive integer in an interval.

	int PositiveInt();
	int PositiveInt(int maxValue);

### PositiveLong
This method returns a 64 bit positive [long](https://docs.microsoft.com/en-us/dotnet/api/system.int64).  
0 is *not* considred a positive number.  
There are overloaded methods creating a positive long in an interval.

	long PositiveLong();
	long PositiveLong(long maxValue);
	long PositiveLong(long minValue, long maxValue);

### String
This method returns a randomised [string](https://docs.microsoft.com/en-us/dotnet/api/system.string).  
There are overloaded methods for prefixing and setting length of the returned string.  
There is a settable property, `StringCharacters`, for which letters are included in the returned string.  
There is a settable property, `StringLength`, for the length of the returned string. Standard length is 8.

	string String();
	string String(int length);
	string String(string prefix);
	string String(string prefix, int length);

#### Examples

	var pr = new PseudoRandom();

	var customerName = pr.String();
	// customerName becomes something like "UtaTrBNd"

	var userName = pr.String(6);
	// userName is something like "UtaTrB"
	// which is the same as setting pr.StringLength = 6;

	var ticketReference = pr.String("TKT-", 10);
	// ticketReference becomes something like "TKT-UtaTrB".

	var tooShort = pr.String("LongPrefix", 5);
	// The lib has decided to play it safe and sets tooShort to "LongP".

	pr.StringCharacters = "012345+-";
	var numbers = pr.String();
	// numbers becomes something like "45-+241+";