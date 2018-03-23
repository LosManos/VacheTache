# VacheTache.FileSystem.FileSystemHelper

## DirectoryEntry

### Examples
    var myDirectory = 
        FileSystemHelper.Directory
        .WithPath( @"C:\Temp\TestLog")
        .MakeExist();

### Constructors

Typically created through  `VacheTache.FileSystem.FileSystemHelper.Directory`.

### Properties

#### Path
This property returns the path without any filename.

### Methods

#### WithPath
Sets up a Directory to create.

#### MakeExist
This is the "act" method where the directory is created if needed or let be if it already exists..

## FileEntity

### Examples
    var myFile =
        FileSystemHelper.File
        .WithPath(@"C:\Temp\TestLog")
        .WithFilename("flag.txt")
        .MakeExist();

### Constructors
Typically created through  `VacheTache.FileSystem.FileSystemHelper.File`.

### Properties

#### Filename
Retrieves the name of the file to be created, with no path.

#### Path
Returns the path for the file to be created, without an filename.

#### Pathfile
Returns the path and the filename of the file to be created, as one string.

#### SizeInBytes
Returns the size of the file to be created.

### Methods

#### WithFilename
Sets up the filename of the file to create.

#### WithPath
Sets up the path of the file to create.

#### WithSize
Sets up the size, in bytes, of the file to create.

#### MakeExist
This is the "act" method where the directory and file is created if needed or let be if it already exists..
