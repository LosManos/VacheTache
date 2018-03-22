using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using VacheTacheLibrary.FileSystem;
using VacheTacheLibrary.FileSystem.Exception;

namespace FileSystemHelperTest
{
    [TestClass]
    public class DirectoryEntityTest : EntityTestBase
    {
        [TestMethod]
        public void WithDirectory_NullParameter_ThrowException()
        {
            //  #   Arrange..
            var sut = FileSystemHelper.Directory;

            //  #   Act and assert.
            Common.Assert.Throws<ArgumentNullException>(() =>
            {
                sut.WithPath(null);
            });
        }

        [TestMethod]
        public void MakeExist_NullPath_ThrowException()
        {
            //  #   Arrange..
            var sut = FileSystemHelper.Directory;

            //  #   Act and Assert.
            var res = Assert.ThrowsException<VacheTacheArgumentNullException>(() =>
            {
                sut.WithPath(null);
            });
            Assert.IsInstanceOfType(res, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void MakeExist_NullPath_ThrowExceptionWhenMakeExist()
        {
            //  #   Arrange.
            var sut = FileSystemHelper.Directory;

            //  #   Act and Asseert.
            var res = Assert.ThrowsException<VacheTacheArgumentNullException>(() =>
            {
                sut.MakeExist();
            });
            Assert.IsInstanceOfType(res, typeof(ArgumentNullException));
        }

        [TestMethod]
        public void GivenPath_SetPathProperty()
        {
            //  #   Arrange.
            var sut = FileSystemHelper.Directory;

            //  #   Act.
            sut.WithPath("mypath");

            //  #   Assert.
            Assert.AreEqual("mypath", sut.Path);
        }

        [TestMethod]
        public void NonExistingDirectory_CreateDirectory()
        {
            //  #   Arrange.
            Common.MakeEmptyPathExist(ThePath);
            var newPath = Path.Combine(ThePath, "AnyDirectory");

            //  #   Act.
            FileSystemHelper.Directory
                .WithPath(newPath)
                .MakeExist();

            //  #   Assert.
            Assert.IsTrue(Directory.Exists(newPath), "The directory should have been created.");
        }

        [TestMethod]
        public void ExistingDirectory_LeaveItBe()
        {
            //  #   Arrange.
            Common.MakeEmptyPathExist(ThePath);
            var newPath = Path.Combine(ThePath, "AnyDirectory");
            var newPathfile = Path.Combine(newPath, Common.AnyFilename(_pr));
            Directory.CreateDirectory(newPath);
            File.Create(newPathfile);

            //  #   Act.
            FileSystemHelper.Directory
                .WithPath(newPath)
                .MakeExist();

            //  #   Assert.
            Assert.IsTrue(File.Exists(newPathfile), "The directory should have been created and the file should still be there.");
        }
    }
}
