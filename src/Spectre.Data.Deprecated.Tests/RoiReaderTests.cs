/*
 * RoiReaderTests.cs
 * Class with tests for RoiReader class.

   Copyright 2017 Roman Lisak

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System.IO;
using NUnit.Framework;
using Spectre.Data.Deprecated.RoiIo;

namespace Spectre.Data.Deprecated.Tests
{
    [TestFixture]
    public class RoiReaderTests
    {
        [Test]
        public void GetAllRoisFromDirectory_returns_the_same_roi_as_on_the_disk_in_alphabetical_order()
        {
            RoiReader service = new RoiReader(DataStub.TestDirectoryPath);
            var allRoisFromDirectory = service.GetAllRoisFromDirectory();
            Assert.AreEqual(actual: allRoisFromDirectory.Count, expected: DataStub.ExpectedNumberOfRoisInDirectory, message: "Incorrect number of rois read from directory");

            Assert.AreEqual(actual: allRoisFromDirectory[0].Name, expected: DataStub.AddRoiDataset.Name);
            Assert.AreEqual(actual: allRoisFromDirectory[1].Name, expected: DataStub.ReadRoiDataset.Name);
            Assert.AreEqual(actual: allRoisFromDirectory[2].Name, expected: DataStub.WriteRoiDataset.Name);

            Assert.AreEqual(actual: allRoisFromDirectory[0].Height, expected: DataStub.AddRoiDataset.Height);
            Assert.AreEqual(actual: allRoisFromDirectory[1].Height, expected: DataStub.ReadRoiDataset.Height);
            Assert.AreEqual(actual: allRoisFromDirectory[2].Height, expected: DataStub.WriteRoiDataset.Height);

            Assert.AreEqual(actual: allRoisFromDirectory[0].Width, expected: DataStub.AddRoiDataset.Width);
            Assert.AreEqual(actual: allRoisFromDirectory[1].Width, expected: DataStub.ReadRoiDataset.Width);
            Assert.AreEqual(actual: allRoisFromDirectory[2].Width, expected: DataStub.WriteRoiDataset.Width);
        }

        [Test]
        public void GetSingleRoiFromDirectoryOrDefault_returns_proper_roi_pixels([Values(0, 1, 2)] int iterator)
        {
            RoiReader service = new RoiReader(DataStub.TestDirectoryPath);
            var roi = service.GetSingleRoiFromDirectoryOrDefault(Path.GetFileNameWithoutExtension(DataStub.TestReadFilePath));

            Assert.AreEqual(actual: roi.RoiPixels[iterator].XCoordinate, expected: DataStub.ReadRoiDataset.RoiPixels[iterator].XCoordinate);
            Assert.AreEqual(actual: roi.RoiPixels[iterator].YCoordinate, expected: DataStub.ReadRoiDataset.RoiPixels[iterator].YCoordinate);
        }

        [Test]
        public void GetSingleRoiFromDirectoryOrDefault_returns_proper_dimensions()
        {
            RoiReader service = new RoiReader(DataStub.TestDirectoryPath);
            var roi = service.GetSingleRoiFromDirectoryOrDefault(Path.GetFileNameWithoutExtension(DataStub.TestReadFilePath));

            Assert.AreEqual(actual: roi.Height, expected: DataStub.ReadRoiDataset.Height);
            Assert.AreEqual(actual: roi.Width, expected: DataStub.ReadRoiDataset.Width);
        }

        [Test]
        public void GetAllNamesFromDirectory_gets_all_names_properly()
        {
            RoiReader service = new RoiReader(DataStub.TestDirectoryPath);

            var names = service.GetAllNamesFromDirectory();

            Assert.AreEqual(actual: Path.GetFileNameWithoutExtension(names[0]), expected: DataStub.AddRoiDataset.Name);
            Assert.AreEqual(actual: Path.GetFileNameWithoutExtension(names[1]), expected: DataStub.ReadRoiDataset.Name);
            Assert.AreEqual(actual: Path.GetFileNameWithoutExtension(names[2]), expected: DataStub.WriteRoiDataset.Name);
        }
    }
}
