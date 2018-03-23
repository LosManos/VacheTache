using System;
using IO = System.IO;

namespace VacheTacheLibrary.FileSystem
{
    public static partial class FileSystemHelper
    {
        public static DirectoryEntity Directory { get { return new DirectoryEntity(); } }

        public class DirectoryEntity
        {
            /// <summary>This property returns the Path only, no Filename.
            /// </summary>
            public string Path { get; private set; }

            public DirectoryEntity WithPath(string path)
            {
                Path = path ?? throw new Exception.VacheTacheArgumentNullException(nameof(path));
                return this;
            }

            /// <summary>This method makes sure the path exists. If the path does not exist beforehand, it is created. If it does exist it is created.
            /// </summary>
            /// <returns></returns>
            public DirectoryEntity MakeExist()
            {
                if( Path == null)
                {
                    throw new Exception.VacheTacheArgumentNullException(nameof(Path));
                }
                else
                {
                    IO.Directory.CreateDirectory(Path);
                }
                return this;
            }
        }
    }
}
