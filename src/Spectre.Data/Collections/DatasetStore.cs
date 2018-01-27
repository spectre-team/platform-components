/*
    * DatasetStore.cs
    * Manages datasets organization on disk

    Copyright 2018 Grzegorz Mrukwa

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

using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Spectre.Data.Readers;
using Spectre.Data.Structures;
using Spectre.Data.Writers;

namespace Spectre.Data.Collections
{
    /// <summary>
    /// Manages datasets organization on disk
    /// </summary>
    public class DatasetStore
    {
        /// <summary>
        /// Gets the root of the store in the filesystem.
        /// </summary>
        /// <value>
        /// The root directory with datasets.
        /// </value>
        public string Root { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatasetStore" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="root">The root of datasets on the disk.</param>
        public DatasetStore(IFileSystem fileSystem, string root)
        {
            Root = root;
            _fileSystem = fileSystem;
            _reader = new TextDatasetReader(_fileSystem);
            _writer = new TextDatasetWriter(_fileSystem);
        }

        /// <summary>
        /// Lists the datasets in the root.
        /// </summary>
        /// <returns>Names of all datasets</returns>
        public IEnumerable<string> ListDatasets() => _fileSystem.Directory.EnumerateDirectories(Root).Select(_fileSystem.Path.GetFileName);

        /// <summary>
        /// Gets the dataset under specified name.
        /// </summary>
        /// <param name="name">The name of the dataset.</param>
        /// <returns>Dataset stored under given name.</returns>
        /// <exception cref="IOException">Could not read dataset.</exception>
        public BasicTextDataset Get(string name) => _reader.Load(ToPath(name));

        /// <summary>
        /// Adds the dataset specified by a name.
        /// </summary>
        /// <param name="name">The name identifying the dataset.</param>
        /// <param name="data">The data.</param>
        public void Add(string name, IDataset data) => _writer.Save(data, ToPath(name));

        /// <summary>
        /// Resolves the path of the dataset by name.
        /// </summary>
        /// <param name="name">The name of the dataset.</param>
        /// <returns>Path to the dataset.</returns>
        private string ToPath(string name) => _fileSystem.Path.Combine(Root, name, "text_data", "data.txt");

        /// <summary>
        /// The file system.
        /// </summary>
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// The reader.
        /// </summary>
        private readonly TextDatasetReader _reader;

        /// <summary>
        /// The writer.
        /// </summary>
        private readonly TextDatasetWriter _writer;
    }
}
