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
The default constructor has a optional long parameter.  
It not provided DateTime.UtcNow.Ticks is used.

### PseudoRandom
The default constructor has a optional string parameter.  
If not provided [CallerMemberName] is used.

## Methods

### Bool
This method resturns a randomised boolean value.

### Date
This method returns a date with time set to midnight.  
There are overloaded methods creating a date in an interval.

### Guid
This method returns a guid.  
Note that the value might not be valid guid according to the [5 variants](https://en.wikipedia.org/wiki/Universally_unique_identifier#Versions) there are.  
Dotnet and Sqlserver recognises it though because 128bits are 128 bits.

### Int
This method returns a 32 bit integer.  
There are overloaded methods creating an integer in an interval.

### PositiveInt
This method returns a 32 bit positive integer. 0 is not considered a positive number.
There are overlaoded methods creating a postivite integer in an interval.
