using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Spectre.Data.Deprecated.Datasets;

namespace Spectre.Data.Deprecated.Tests
{
    public static class DataStub
    {
        private static string FindTestDirectory(string current = null)
        {
            current = current ?? Directory.GetCurrentDirectory();
            var expectedLocation = System.IO.Path.Combine(current, "test_files", "Rois");
            if (Directory.Exists(expectedLocation))
            {
                return expectedLocation;
            }

            return FindTestDirectory(System.IO.Path.Combine(current, ".."));
        }

        static readonly string Path = FindTestDirectory(TestContext.CurrentContext.TestDirectory);
        public static string TestDirectoryPath = System.IO.Path.GetFullPath(DataStub.Path);
        public static string TestReadFilePath = System.IO.Path.Combine(DataStub.TestDirectoryPath, "image1.png");
        public static string TestWriteFilePath = System.IO.Path.Combine(DataStub.TestDirectoryPath, "writetestfile.png");
        public static int ExpectedNumberOfRoisInDirectory = 3;

        public static Roi ReadRoiDataset = new Roi("image1", 6, 6, new List<RoiPixel>
        {
            new RoiPixel(1, 1),
            new RoiPixel(2, 1),
            new RoiPixel(3, 1)
        });

        public static Roi WriteRoiDataset = new Roi("writetestfile", 10, 10, new List<RoiPixel>
        {
            new RoiPixel(1, 5),
            new RoiPixel(2, 5),
            new RoiPixel(3, 5),
            new RoiPixel(4, 5)
        });

        public static Roi AddRoiDataset = new Roi("addtestfile", 10, 10, new List<RoiPixel>
        {
            new RoiPixel(1, 6),
            new RoiPixel(2, 6),
            new RoiPixel(3, 6),
            new RoiPixel(4, 6)
        });

    }
}
