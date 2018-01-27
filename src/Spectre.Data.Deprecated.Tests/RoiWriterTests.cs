/*
 * RoiWriterTests.cs
 * Class with tests for RoiWriter class.

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
    public class RoiWriterTests
    {
        [Test]
        public void WriteRoi_writes_file_properly([Values(0, 1, 2, 3)] int iterator)
        {
            RoiWriter service = new RoiWriter();
            service.RoiWriterTool(DataStub.WriteRoiDataset, DataStub.TestDirectoryPath);
            RoiReader reader = new RoiReader(DataStub.TestDirectoryPath);
            var writetestroi = reader.GetSingleRoiFromDirectoryOrDefault(Path.GetFileNameWithoutExtension(DataStub.TestWriteFilePath));

            Assert.AreEqual(actual: writetestroi.RoiPixels[iterator].XCoordinate, expected: DataStub.WriteRoiDataset.RoiPixels[iterator].XCoordinate);
            Assert.AreEqual(actual: writetestroi.RoiPixels[iterator].YCoordinate, expected: DataStub.WriteRoiDataset.RoiPixels[iterator].YCoordinate);
        }
    }
}
