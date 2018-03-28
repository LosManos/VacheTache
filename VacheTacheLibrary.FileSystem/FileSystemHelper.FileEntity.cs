using IO = System.IO;

namespace VacheTacheLibrary.FileSystem
{
    public static partial class FileSystemHelper
    {
        public static FileEntity File { get { return new FileEntity(); } }

        public class FileEntity
        {
            /// <summary>This property returns the Filename including suffix.
            /// </summary>
            public string Filename { get; private set; }

            /// <summary>This proprerty returns the Path only, no Filename.
            /// </summary>
            public string Path { get; private set; }

            /// <summary>This property returns the Pathfile; that is Path and Filename.
            /// </summary>
            public string Pathfile { get { return IO.Path.Combine(Path ?? string.Empty, Filename); } }

            /// <summary>This property returns the size, in bytes.
            /// </summary>
            public long SizeInBytes { get; private set; }

            public FileEntity WithFilename(string filename)
            {
                if (filename == null)
                {
                    throw new Exception.VacheTacheArgumentNullException(nameof(filename));
                }
                var x = SplitToPathAndFilename(filename);
                if (string.IsNullOrWhiteSpace(x.Path) == false)
                {
                    Path = x.Path;
                    Filename = x.Filename;
                }
                else
                {
                    Filename = filename;
                }

                return this;
            }

            public FileEntity WithPath(string path)
            {
                Path = path;
                return this;
            }

            public FileEntity WithSize(long bytes)
            {
                SizeInBytes = bytes;
                return this;
            }

            /// <summary>This method makes sure the path, and possible file, exists. If the path/file does not exist beforehand, it/they are created. If anone of them does not exist it/they is created.
            /// </summary>
            /// <returns></returns>
            public FileEntity MakeExist()
            {
                if (Filename == null)
                {
                    throw new Exception.VacheTacheArgumentNullException(nameof(Filename));
                }

                if (Path != null)
                {
                    IO.Directory.CreateDirectory(Path);
                }

                if (Filename != null)
                {
                    if (IO.File.Exists(Pathfile) == false ||
                        new IO.FileInfo(Pathfile).Length != SizeInBytes)
                    {
                        MakeExistFile(Pathfile, SizeInBytes);
                    }
                }
                return this;
            }

            #region Private helper methods.

            /// <summary>This method makes certain a file of a certain size exists.
            /// The file can exist beforehand.
            /// </summary>
            /// <param name="pathfile"></param>
            /// <param name="sizeInBytes"></param>
            private static void MakeExistFile(string pathfile, long sizeInBytes)
            {
                IO.File.Delete(pathfile);
                WriteFile(pathfile, sizeInBytes);
            }

            /// <summary>This method splits a pathfile to a Path and a File.
            /// </summary>
            /// <param name="pathfile"></param>
            /// <returns></returns>
            private static (string Path, string Filename) SplitToPathAndFilename(string pathfile)
            {
                return (IO.Path.GetDirectoryName(pathfile), IO.Path.GetFileName(pathfile));
            }

            /// <summary>This method writes a file of a cetain size.
            /// The file must not exist beforehand.
            /// </summary>
            /// <param name="pathfile"></param>
            /// <param name="sizeInBytes"></param>
            private static void WriteFile(
                string pathfile,
                long sizeInBytes)
            {
                // There is certainly a faster way to create the file
                // but his will do for now.
                using (var stream = IO.File.Create(pathfile))
                {
                    for (var n = 0; n < sizeInBytes; ++n)
                    {
                        //stream.Write(new[] { (byte)42 }, 0, 1);
                        stream.WriteByte(42);
                    }
                }
            }

            #endregion
        }
    }
}
