/*
    * RoiDictionary.cs
    * Class representing a set of tool to manage ROIs.
    * Allows getting ROIs by name, adding and removing by name.

    Copyright 2017 Roman LIsak

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
using Spectre.Data.Deprecated.RoiIo;

namespace Spectre.Data.Deprecated.Datasets
{
    /// <summary>
    /// Class representing a set of tools to manage ROIs.
    /// </summary>
    /// <seealso cref="IRoiDictionary" />
    public class RoiDictionary : IRoiDictionary
    {
        private readonly RoiReader _roireader;
        private readonly string _directoryPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoiDictionary"/> class.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        public RoiDictionary(string directoryPath)
        {
            _directoryPath = directoryPath;
            _roireader = new RoiReader(_directoryPath);
        }

        /// <summary>
        /// Loads all Rois from directory to the list.
        /// </summary>
        /// <returns>
        /// Dataset with all rois.
        /// </returns>
        public IList<Roi> LoadAllRois() => _roireader.GetAllRoisFromDirectory();

        /// <summary>
        /// Gets specified single roi from directory.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        /// Returns roi with specified name.
        /// Returns null if no roi with specified name found.
        /// </returns>
        public Roi LoadSingleRoiOrDefault(string fileName) => _roireader.GetSingleRoiFromDirectoryOrDefault(fileName);

        /// <summary>
        /// Adds the specified ROI to the dictionary and creates file on the disk.
        /// </summary>
        /// <param name="roi">The roi.</param>
        public void Add(Roi roi)
        {
            var roiWriterService = new RoiWriter();
            roiWriterService.RoiWriterTool(roi, _directoryPath);
        }

        /// <summary>
        /// Removes the ROI with specified name from dictionary and from disk.
        /// </summary>
        /// <param name="name">The name.</param>
        public void Remove(string name)
        {
            File.Delete(Path.Combine(_directoryPath, name + ".png"));
        }

        /// <summary>
        /// Gets the roi names from list.
        /// </summary>
        /// <returns>Names of all rois in dictionary.</returns>
        public IList<string> GetRoiNames() => _roireader.GetAllNamesFromDirectory();
    }
}
