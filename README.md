# VacheTache

Version 2.0.0  

## VacheTache class

`VacheTache` is a class for randomising different types of data.

[`VacheTache` documentation](//github.com/LosManos/VacheTache/blob/master/VacheTache.md)

## VacheTache.FileSystem class

Class `VacheTache.FileSystem.FileSystemHelper` contains classes for creating folders and files in an easy way in automatic testing environments.

[`VacheTache.FileSystem.FileSystemHelper` documentation](//github.com/LosManos/VacheTache/blob/master/VacheTache.FileSystem.FileSystemHelper.md)

## PseudoRandom class

`PseudoRandom` has a slightly different raison d'ètre than `VacheTache`.  
Namely to *not* generate randomised data but determistic data. This is for instance used for automatic tests where one wants to know the input and hence output.  
Technically it is really just a descendant of `VacheTache` with a nifty constructor and a name that reminds of its purpose.

[`PseudoRandom` documentation](//github.com/LosManos/VacheTache/blob/master/PseudoRandom.md)

## Nuget
https://www.nuget.org/packages/VacheTache/

## License
This library is licensed under [LGPLv3+NoEvil license](https://raw.githubusercontent.com/LosManos/VacheTache/master/License.txt).  
This means that you are free to use this library in your (commercial) product as long as  you
1) attach the code for VacheTache
1) and any updates you have made to VacheTache
1) and the license for VacheTache  
1) and not use the library with a company that sells or buys munition
1) and not use the library with a company in a country  that allows capital punishment

to every customer of your (commercial) product.  
Your existing (commercial) product is not affected.

## Stability

Most methods are ok but some are the first naïve implementation.  
With that said, many of the methods have been copied to a real system where they, by the time of writing, are running. Or at least lat time I talked to them.
