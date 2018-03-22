using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using VacheTacheLibrary;

namespace FileSystemHelperTest
{
    internal static class Common
    {
        /// <summary>This helper method returns an "any" filename
        /// which is a (determinastically) randomised string.
        /// The length 14 just happens to be any length
        /// and the filename lacks suffix as the test is not affected by suffix or not.
        /// </summary>
        /// <returns></returns>
        internal static string AnyFilename(PseudoRandom pr)
        {
            return pr.String(14);
        }

        /// <summary>This helper method returns an "any" director name
        /// which is a (determinastically) randomised string.
        /// The length 12 just happens to be any length.
        /// </summary>
        /// <returns></returns>
        internal static string AnyDirectoryName(PseudoRandom pr)
        {
            return pr.String(12);
        }

        internal static void MakeEmptyPathExist(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsFalse(Directory.Exists(path),
                "Sanity check: During the setup the path should be deleted right now but will be created again later.");
            Directory.CreateDirectory(path);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.IsTrue(Directory.Exists(path),
                "Sanity check: The directory should exist.");
        }

        /// <summary>This helper method is for allowing to test for a thrown exception.
        /// See <see cref="https://stackoverflow.com/a/5634337/521554"/>
        /// </summary>
        public static class Assert
        {
            public static void Throws<T>(Action func, string message = null) where T : Exception
            {
                var exceptionThrown = false;
                try
                {
                    func.Invoke();
                }
                catch (T)
                {
                    exceptionThrown = true;
                }

                if (exceptionThrown == false)
                {
                    throw new AssertFailedException(
                        $"An exception of type {typeof(T)} was expected, but not thrown." +
                        (message == null ? string.Empty : Environment.NewLine + message)
                        );
                }
            }
        }
    }
}
