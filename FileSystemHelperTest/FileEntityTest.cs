using CompulsoryCow;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using VacheTacheLibrary.FileSystem;
using VacheTacheLibrary.FileSystem.Exception;

namespace FileSystemHelperTest
{
    [TestClass]
    public class FileEntityTest : EntityTestBase
    {
        [TestMethod]
        public void WithFilename_NullParameter_ThrowException()
        {
            //  #   Arrange..
            var sut = FileSystemHelper.File;

            //  #   Act and assert.
            var res = Assert.ThrowsException<VacheTacheArgumentNullException>(() =>
            {
                sut.WithFilename(null);
            });
            Assert.IsInstanceOfType(res, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void WithoutFilename_ThrowExceptionWhenMakeExist()
        {
            //  #   Arrange..
            var sut = FileSystemHelper.File;

            //  #   Act and assert.
            var res = Assert.ThrowsException<VacheTacheArgumentNullException>(() =>
            {
                sut.MakeExist();
            });
            Assert.IsInstanceOfType(res, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void MakeExist_NullPath_CreateAtCurrentPath()
        {
            //  #   Arrange..
            const string Filename = "Whatever";
            var sut = FileSystemHelper.File
                .WithFilename(Filename);
            var currentPath = Directory.GetCurrentDirectory();
            var currentPathfile = Path.Combine(currentPath, Filename);
            MakeFileNotExist(currentPathfile);

            //  #   Act.
            sut.MakeExist();

            //  #   Assert.
            Assert.IsTrue(File.Exists(currentPathfile));
        }

        [TestMethod]
        public void GivenFilename_SetFilenameProperty()
        {
            //  #   Arrange.
            var sut = FileSystemHelper.File;
            var anyFilename = Common.AnyFilename(_pr);

            //  #   Act.
            sut.WithFilename(anyFilename);

            //  #   Assert.
            Assert.AreEqual(anyFilename, sut.Filename);
        }

        [TestMethod]
        public void GivenPath_SetPathProperty()
        {
            //  #   Arrange.
            var sut = FileSystemHelper.File;
            var anyDirectoryName = Common.AnyDirectoryName(_pr);

            //  #   Act.
            sut.WithPath(anyDirectoryName);

            //  #   Assert.
            Assert.AreEqual(anyDirectoryName, sut.Path);
        }

        [TestMethod]
        public void GivenSize_SetSizeProperty()
        {
            //  #   Arrange.
            var sut = FileSystemHelper.File;
            var anyDirectoryName = Common.AnyDirectoryName(_pr);

            //  #   Act.
            sut.WithSize(44);

            //  #   Assert.
            Assert.AreEqual(44, sut.SizeInBytes);
        }

        [TestMethod]
        public void FilenameForNonExistingFile_CreateFile()
        {
            //  #   Arrange.
            Common.MakeEmptyPathExist(ThePath);

            var filename = _pr.String(14);
            var pathfile = Path.Combine(ThePath, filename);

            //  #   Act.
            var res = FileSystemHelper.File
                .WithFilename(pathfile)
                .MakeExist();

            //  #   Assert.
            Assert.IsTrue(File.Exists(pathfile), "The file should have been created.");
        }

        [TestMethod]
        public void FilenameForExistingFile_NotTouchDirectoryOrFile()
        {
            //  #   Arrange.
            Common.MakeEmptyPathExist(ThePath);
            var preexistingFilepath = Path.Combine(ThePath, Common.AnyFilename(_pr));
            MakeFileExist(preexistingFilepath);
            var otherExistingFilepathInDirectory =Path.Combine(ThePath, Common.AnyFilename(_pr));
            MakeFileExist(otherExistingFilepathInDirectory);

            //  #   Act.
            var res = FileSystemHelper.File
                .WithFilename(preexistingFilepath)
                .MakeExist();

            //  #   Assert.
            Assert.IsTrue(File.Exists(preexistingFilepath), "This file should exist, just like the Act call makes it.");
            Assert.IsTrue(File.Exists(otherExistingFilepathInDirectory), "Another file in the same folder should not be touched. This check makes sure the directory is not deleted and recreated.");
        }

        [TestMethod]
        public void FilenameForExistingFileWithOtherSize_UpdateFile()
        {
            //  #   Arrange.
            Common.MakeEmptyPathExist(ThePath);
            var preexistingFilepath = Path.Combine(ThePath, Common.AnyFilename(_pr));
            MakeFileExist(preexistingFilepath, 12);

            //  #   Act.
            var res = FileSystemHelper.File
                .WithFilename(preexistingFilepath)
                .WithSize(24)
                .MakeExist();

            //  #   Assert.
            Assert.IsTrue(File.Exists(preexistingFilepath), "This file should exist, just like the Act call makes it.");
            Assert.AreEqual(24, new FileInfo(preexistingFilepath).Length);
        }

        [TestMethod]
        public void FilenameForExistingFileWithSameSize_NotTouchFile()
        {
            //  #   Arrange.
            Common.MakeEmptyPathExist(ThePath);
            var preexistingFilepath = Path.Combine(ThePath, Common.AnyFilename(_pr));
            MakeFileExist(preexistingFilepath, 12);
            var otherExistingFilepathInDirectory = Path.Combine(ThePath, Common.AnyFilename(_pr));
            MakeFileExist(otherExistingFilepathInDirectory);

            //  #   Act.
            var res = FileSystemHelper.File
                .WithFilename(preexistingFilepath)
                .WithSize(12)
                .MakeExist();

            //  #   Assert.
            Assert.IsTrue(File.Exists(preexistingFilepath), "This file should exist, just like the Act call makes it.");
            Assert.IsTrue(File.Exists(otherExistingFilepathInDirectory), "Another file in the same folder should not be touched. This check makes sure the directory is not deleted and recreated.");
            Assert.AreEqual(12, res.SizeInBytes);
        }

        [TestMethod]
        public void PathfileInNonExistingPath_CreateFolderAndFile()
        {
            //  #   Arrange.
            Common.MakeEmptyPathExist(ThePath);
            var filename = _pr.String(14);
            var path = Path.Combine(ThePath, _pr.String(14)); // This is a folder that does not exist.
            var pathfile = Path.Combine(path, filename);
            Assert.IsFalse(Directory.Exists(path), "Sanity check: The path should not exist, but it does.");

            //  #   Act.
            var res = FileSystemHelper.File
                .WithFilename(pathfile)
                .MakeExist();

            //  #   Assert.
            Assert.IsTrue(File.Exists(pathfile), "The file should have been created.");
        }

        [TestMethod]
        public void Size_CreateFileOfSaidSize()
        {
            //  #   Arrange.
            var pathfile = Path.Combine(ThePath, nameof(Size_CreateFileOfSaidSize));
            MakeFileNotExist(pathfile);

            //  #   Act.
            var res = FileSystemHelper.File
                .WithPath(ThePath)
                .WithFilename(nameof(Size_CreateFileOfSaidSize))
                .WithSize(42)
                .MakeExist();

            //  #   Assert.
            Assert.AreEqual(42, new FileInfo(pathfile).Length);
        }

        [TestMethod]
        public void WithFilename_FilenameWIthPath_SetPath()
        {
            //  #   Arrange.
            var sut = FileSystemHelper.File;

            var pathfile = IsWindows() ?
                 @"C:\MyDirectory\MyFile.txt" :
                 @"/mnt/c/MyDirectory/MyFile.txt";
            var expectedPath = IsWindows() ?
                @"C:\MyDirectory" :
                @"/mnt/c/MyDirectory";

            //  #   Act..
            sut.WithFilename(pathfile);
            
            //  #   Assert.
            Assert.AreEqual(expectedPath, sut.Path, "The Path should have been set by the Filename since it contains a path.");
        }

        [TestMethod]
        public void SplitPathAndFilename_pathandfile_ReturnPathAndFilename()
        {
            //  #   Arrange.
            dynamic sutPrivate = new ReachPrivateIn(typeof(FileSystemHelper.FileEntity));
            var pathfile = IsWindows() ?
                @"C:\Temp\File.txt" :
                "/mnt/c/Temp/File.txt";
            var expectedPath = IsWindows() ?
                @"C:\Temp" :
                @"/mnt/c/Temp";

            //  #   Act.
            var res = sutPrivate.SplitToPathAndFilename(pathfile);

            //  #   Assert.
            Assert.AreEqual(expectedPath, res.Item1);
            Assert.AreEqual("File.txt", res.Item2);
        }

        private static bool IsWindows() =>
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(
                System.Runtime.InteropServices.OSPlatform.Windows);

        /// <summary>Helper method that makes sure a path exist.
        /// Do not mistake it for the testees similar functionality
        /// as this  is the test's helper method.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="message"></param>
        private static void MakePathExist(string path, string message = null)
        {
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
            Assert.IsTrue(Directory.Exists(path),
                message ?? $"Sanity check: The path [{path}] should exist. It does not.");
        }

        private static void MakeFileExist(
            string pathfile, 
            int sizeInBytes = 0,
            string message = null)
        {
            MakePathExist(Path.GetDirectoryName(pathfile));
            if( File.Exists(pathfile) == false)
            {
                using (var stream = File.Create(pathfile))
                {
                    for(var n = 0; n < sizeInBytes; ++n)
                    {
                        stream.WriteByte((byte)'A');
                    }
                }
            }
            Assert.IsTrue(File.Exists(pathfile),
                message ?? $"Sanity check: The [{pathfile}] should exist. It does not.");
        }

        private static void MakeFileNotExist( string pathfile, string message = null)
        {
            if (File.Exists(pathfile))
            {
                File.Delete(pathfile);
            }
            Assert.IsFalse(File.Exists(pathfile),
                message ?? $"Sanity check: The file [{pathfile}] should not exist. It does.");
        }

        private static void MakePathNotExist(string path, string message = null)
        {
            if(Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Assert.IsFalse(Directory.Exists(path),
                message ?? $"Sanity check: The path [{path}] should not exist. It does.");
        }
    }
}
