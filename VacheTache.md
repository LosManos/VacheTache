# VacheTache

## Constructors

The default constructor has an optional long parameter used for randomising the seed.  
It not provided `DateTime.UtcNow.Ticks` is used.  
This makes it easy to create the class and have randomised values.

	VacheTache(long? seed = null);
	VacheTache(Func<int,int> hashCodeFunc, long seed);

### Examples

	var vt = new VacheTache();
	var address = vt.String("ADR-");
	// address is now something like "ADR-xdIC".

## Methods

### Bool
This method resturns a randomised [Boolean](https://docs.microsoft.com/en-us/dotnet/api/system.boolean) value.

	bool Bool();

### Date
This method returns a [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime) with time set to midnight.  
There are overloaded methods creating a date in an interval.

	DateTime Date();
	DateTime Date(DateTime from, DateTime to);

### DateAndTime
This method returns a [DateTime](https://docs.microsoft.com/en-us/dotnet/api/system.datetime) where time is hours, minutes and seconds. Anything finer grained than a second is 0.

	DateTime DateAndTime();
	DateTime DateAndTime(DateTime from, DateTime to);

### Decimal
This method returns a (C#) [Decimal](https://docs.microsoft.com/en-us/dotnet/articles/csharp/language-reference/keywords/decimal).  
The result is probably not evenly distributed.  
There are some worries around `Decimal(decimal minValue, decimal maxValue)` so it is *not* implemented.

	decimal Decimal();

### Currency
This method(s) returns a natural number or a natural number and 2 decimal fraction. The former is for use with currencies without decimals, i.e. yen. 
To set the number of decimals, one either does that in the call or in the property `NumberOfCurrencyDecimals`.

	decimal Currency();
	decimal Currency(int min, int max);
	decimal Currency(int min, int max, int decimals);

**TBA**

* Properties `CurrencyMin` and `CurrencyMax` for setting the min and max in a central, convenient place.
* Generate currencies in even 5 or 10 intervals.

### Enum
This method returns an [`Enum`](https://msdn.microsoft.com/en-us/library/system.enum).

	TEnum Enum<TEnum>();

Note that it, presently, does not handle inconsecutive value series or items with the same value.

### EnumExcept
This method returns an enum except the one provided in the parameter.

	TEnum EnumExcept<TEnum>(IEnumerable<TEnum> exceptEnums)where TEnum : struct

### Guid
This method returns a [`Guid`](https://docs.microsoft.com/en-us/dotnet/api/system.guid).  
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
This method returns a 32 bit positive [`int`](https://docs.microsoft.com/en-us/dotnet/api/system.int32).  
0 is *not* considered a positive number.  
There are overloaded methods creating a positive integer in an interval.

	int PositiveInt();
	int PositiveInt(int maxValue);

### PositiveLong
This method returns a 64 bit positive [`long`](https://docs.microsoft.com/en-us/dotnet/api/system.int64).  
0 is *not* considered a positive number.  
There are overloaded methods creating a positive `long` in an interval.

	long PositiveLong();
	long PositiveLong(long maxValue);
	long PositiveLong(long minValue, long maxValue);

### String
This method returns a randomised [`string`](https://docs.microsoft.com/en-us/dotnet/api/system.string).  
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
