/*
 * TextDatasetWriter.cs
 * Writes dataset to text file.
 *
   Copyright 2018 Dariusz Kuchta, Michał Gallus, Grzegorz Mrukwa

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

using System.Globalization;
using System.IO.Abstractions;
using System.Text;
using System.Threading;
using Spectre.Data.Structures;

namespace Spectre.Data.Writers
{
    /// <summary>
    /// Writes dataset to text file.
    /// </summary>
    public class TextDatasetWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextDatasetWriter"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        public TextDatasetWriter(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Saves the specified dataset.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="path">The path.</param>
        public void Save(IDataset dataset, string path)
        {
            var fileBuilder = new StringBuilder();

            // fixes comma-instead-of-dot related issues
            BeginInvariantCulture();
            {
                AppendMetadata(fileBuilder, dataset.Metadata);
                AppendMzValues(fileBuilder, dataset.GetRawMzArray());
                AppendIntensitiesAndLocalMetadata(fileBuilder, dataset.GetRawSpacialCoordinates(), dataset.GetRawIntensities());

                SaveDataToFile(path, fileBuilder);
            }
            EndInvariantCulture();
        }

        #region Private methods

        private void SaveDataToFile(string path, StringBuilder fileBuilder)
        {
            using (var sw = _fileSystem.File.CreateText(path))
            {
                sw.Write(value: fileBuilder.ToString());
            }
        }

        private void AppendIntensitiesAndLocalMetadata(StringBuilder fileBuilder, int[,] coordinates, double[,] intensities)
        {
            var spectrumBuilder = new StringBuilder();
            
            for (var i = 0; i < intensities.GetLength(0); ++i)
            {
                fileBuilder.AppendLine(value: $"{coordinates[i, 0]} {coordinates[i, 1]} {coordinates[i, 2]}");

                for (var j = 0; j < intensities.GetLength(1); ++j)
                {
                    spectrumBuilder.Append(value: intensities[i, j]);
                    spectrumBuilder.Append(value: ' ');
                }
                fileBuilder.AppendLine(value: spectrumBuilder.ToString());
                spectrumBuilder.Clear();
            }
        }

        private void AppendMzValues(StringBuilder fileBuilder, double[] mzs)
        {
#pragma warning disable SA1305 // Field names must not use Hungarian notation
            var mzValuesString = new StringBuilder();
#pragma warning restore SA1305 // Field names must not use Hungarian notation
            foreach (var mz in mzs)
            {
                mzValuesString.Append(mz);
                mzValuesString.Append(value: ' ');
            }
            fileBuilder.AppendLine(value: mzValuesString.ToString());
        }

        private void AppendMetadata(StringBuilder fileBuilder, Metadata metadata)
        {
            fileBuilder.AppendLine(metadata.Description);
        }

        private void EndInvariantCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
        }

        private void BeginInvariantCulture()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        #endregion

        private readonly IFileSystem _fileSystem;
    }
}
