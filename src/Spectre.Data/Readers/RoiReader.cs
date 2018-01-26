/*
 * RoiReader.cs
 * Class with utilities for listing ROI from specified folder
 * and reading the regions of interest data from specified file.

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

using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using Spectre.Data.Conversions;
using Spectre.Data.Structures;

namespace Spectre.Data.Readers
{
    /// <summary>
    /// Provides listing all available ROI's from directory.
    /// Provides reading a ROI from a directory.
    /// </summary>
    public class RoiReader
    {
        /// <summary>
        /// The path to the Rois directory.
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// The roi converter
        /// </summary>
        private readonly RoiConverter _roiConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoiReader"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public RoiReader(string path)
        {
            _roiConverter = new RoiConverter();
            _path = path;
        }

        /// <summary>
        /// Lists the rois from directory.
        /// </summary>
        /// <returns>
        /// All ROIs in specified directory.
        /// </returns>
        /// <exception cref="FileNotFoundException">No *.png files found in directory or subdirectories</exception>
        public List<Roi> GetAllRoisFromDirectory()
        {
            var names = GetAllNamesFromDirectory();
            var rois = names.Select(name => GetSingleRoiFromDirectoryOrDefault(Path.Combine(_path, name))).ToList();

            return rois;
        }

        /// <summary>
        /// Gets names of all ROIs from directory.
        /// </summary>
        /// <returns>
        /// Returns names of all files in specified directory.
        /// </returns>
        /// <exception cref="FileNotFoundException">No *.png files found in directory.</exception>
        public List<string> GetAllNamesFromDirectory()
        {
            var allfiles = Directory.GetFiles(_path, "*.png", SearchOption.TopDirectoryOnly);

            for (int iterator = 0; iterator < allfiles.Length; iterator++)
            {
                allfiles[iterator] = Path.GetFileNameWithoutExtension(allfiles[iterator]);
            }

            var names = allfiles.ToList();
            names.Sort();

            return names;
        }

        /// <summary>
        /// Loads single ROI from specified folded.
        /// </summary>
        /// <param name="fileName">Name of the file without extension.</param>
        /// <returns>
        /// ROI dataset.
        /// </returns>
        public Roi GetSingleRoiFromDirectoryOrDefault(string fileName)
        {
            if (!File.Exists(Path.Combine(_path, fileName + ".png")))
            {
                return null;
            }

            using (var bitmap = Image.Load<Rgba32>(Path.Combine(_path, fileName + ".png")))
            {
                var roidataset = _roiConverter.BitmapToRoi(bitmap, Path.GetFileNameWithoutExtension(fileName));
                return roidataset;
            }
        }
    }
}
