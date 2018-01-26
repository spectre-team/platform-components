/*
 * DatasetLoaderTest.cs
 * Tests for dataset loader class.
 *
   Copyright 2017 Dariusz Kuchta
   
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
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using NUnit.Framework;
using Spectre.Dependencies.Deprecated;
using Spectre.Dependencies.Deprecated.Modules;
using Spectre.Service.Deprecated.Configuration;
using Spectre.Service.Deprecated.Io;

namespace Spectre.Service.Deprecated.Tests.Io
{
    [TestFixture]
    public class DatasetLoaderTest
    {
        private DataRootConfig _rootConfig;
        private DatasetLoader _datasetLoader;
        private MockFileSystem _mockFileSystem;

        private readonly string _rootDir = @"C:\spectre_data";
        private readonly string _localDir = "local";
        private readonly string _remoteDir = "remote";

        private const string TestDirectoryName = "test_files";

        private static string FindTestDirectory(string current = null)
        {
            current = current ?? Directory.GetCurrentDirectory();
            var expectedLocation = Path.Combine(current, TestDirectoryName);
            if (Directory.Exists(expectedLocation))
            {
                return expectedLocation;
            }

            return FindTestDirectory(Path.Combine(current, ".."));
        }

        private readonly string _fileDir = FindTestDirectory(TestContext.CurrentContext.TestDirectory);
        [OneTimeSetUp]
        public void SetUp()
        {
            DependencyResolver.AddModule(new MockModule());

            var localDirFull = Path.Combine(_rootDir, _localDir);
            var remoteDirFull = Path.Combine(_rootDir, _remoteDir);
            var correctDataset = File.ReadAllText(Path.Combine(_fileDir, "small-test.txt"));

            _mockFileSystem = DependencyResolver.GetService<IFileSystem>() as MockFileSystem;

            _mockFileSystem.AddFile(Path.Combine(localDirFull, "local_correct.txt"), new MockFileData(correctDataset));
            _mockFileSystem.AddFile(Path.Combine(remoteDirFull, "remote_correct.txt"), new MockFileData(correctDataset));
            _mockFileSystem.AddFile(Path.Combine(localDirFull, "local_incorrect.txt"), new MockFileData(textContents: "incorrect_data"));
            _mockFileSystem.AddFile(Path.Combine(remoteDirFull, "remote_incorrect.txt"), new MockFileData(textContents: "incorrect_data"));

            _rootConfig = new DataRootConfig(localDirFull, remoteDirFull);
            _datasetLoader = new DatasetLoader(_rootConfig);
        }

        [Test]
        public void ReturnsFromCorrectNameLocal()
        {
            Assert.IsNotNull(anObject: _datasetLoader.GetFromName(name: "local_correct"),
                message: "Loader did not manage to load local file.");
        }

        [Test]
        public void ReturnsFromCorrectNameRemote()
        {
            Assert.IsNotNull(anObject: _datasetLoader.GetFromName(name: "remote_correct"),
                message: "Loader did not manage to load remote file.");
        }

        [Test]
        public void ThrowsOnIncorrectName()
        {
            Assert.Throws<DatasetNotFoundException>(code: () => _datasetLoader.GetFromName(name: "invalid_name"),
                message: "Loader accepted invalid file name.");
        }

        [Test]
        public void ThrowsOnIncorrectFileContents()
        {
            Assert.Throws<DatasetFormatException>(code: () => _datasetLoader.GetFromName(name: "local_incorrect"),
                message: "Loader did not manage to load remote file.");
        }

        [Test]
        public void DeletesIncorrectFilesFromLocal()
        {
            try
            {
                _datasetLoader.GetFromName(name: "remote_incorrect");
            }
            catch (DatasetFormatException)
            {
                // ignored
            }
            var result = _mockFileSystem.AllFiles.FirstOrDefault(predicate: file => file.Contains(value: Path.Combine(_localDir, "remote_incorrect")));
            Assert.IsNull(result, message: "Loader leaves copies of incorrect files in local directory.");
        }
    }
}
