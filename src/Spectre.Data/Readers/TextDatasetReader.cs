/*
 * TextDatasetReader.cs
 * Reads dataset from text file.
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Spectre.Data.Structures;

namespace Spectre.Data.Readers
{
    /// <summary>
    /// Reads dataset from text file.
    /// </summary>
    public class TextDatasetReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextDatasetReader"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        public TextDatasetReader(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Loads the dataset from specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Dataset from file</returns>
        /// <exception cref="IOException">
        /// File format is broken.
        /// </exception>
        public virtual BasicTextDataset Load(string path)
        {
            try
            {
                using (var sr = _fileSystem.File.OpenText(path))
                {
                    sr.ReadLine(); // omit global metadata
                    var mz = TextDatasetReader.ParseFloats(sr.ReadLine());
                    
                    var location = new List<int[]>();
                    var data = new List<double[]>();
                    while (sr.Peek() > -1)
                    {
                        (var intensities, var metadata) = ReadSpectrum(sr, mz);
                        location.Add(metadata);
                        data.Add(intensities);
                    }

                    var coordinates = Gather(location);
                    var molecularData = Gather(data);

                    return new BasicTextDataset(mz, molecularData, coordinates);
                }
            }
            catch (NullReferenceException e)
            {
                throw new IOException(
                    message: "M/z data could not be parsed from file " + path + ".",
                    innerException: e);
            }
            catch (InvalidDataException e)
            {
                throw new IOException(message: "Length mismatch in parsed data.", innerException: e);
            }
            catch (Exception e)
            {
                // catch remaining Exceptions that are related to StreamReader
                throw new IOException(message: "Streamer failed to read " + path + " file.", innerException: e);
            }
        }

        private static (double[] intensities, int[] coordinates) ReadSpectrum(TextReader sr, double[] mz)
        {
            var metadata = ParseCoordinates(sr.ReadLine());
            var intensities = ParseFloats(sr.ReadLine());
            if (intensities.Length != mz.Length)
            {
                throw new InvalidDataException(
                    message: "Length of the data must be equal to length of m/z values.");
            }
            return new ValueTuple<double[], int[]>(intensities, metadata);
        }

        private static T[,] Gather<T>(List<T[]> list)
        {
            var width = list.First().Length;
            if (list.Any(c => c.Length != width))
            {
                throw new InvalidDataException("Lengths mismatch in parsed data.");
            }
            var matrix = new T[list.Count, width];
            for (var i = 0; i < list.Count; ++i)
                for (var j = 0; j < width; ++j)
                    matrix[i, j] = list[i][j];
            return matrix;
        }

        private static double[] ParseFloats(string line) => line
            .Split()
            .Where(predicate: str => !string.IsNullOrEmpty(str))
            .Select(d => double.TryParse(d, NumberStyles.Any, CultureInfo.InvariantCulture, out var res)
                ? res
                : double.NaN)
            .ToArray();

        private static int[] ParseCoordinates(string line) => line
            .Split()
            .Where(predicate: str => !string.IsNullOrEmpty(str))
            .Take(3)
            .Select(i => int.TryParse(i, NumberStyles.Any, CultureInfo.InvariantCulture, out var res)
                ? res
                : -1)
            .ToArray();

        private readonly IFileSystem _fileSystem;
    }
}
