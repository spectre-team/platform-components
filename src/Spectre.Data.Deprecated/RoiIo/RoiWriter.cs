/*
 * RoiWriter.cs
 * Class with utilities for writing to a file the regions of interest data.

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

using System.Drawing.Imaging;
using System.IO;
using Spectre.Data.Deprecated.Datasets;

namespace Spectre.Data.Deprecated.RoiIo
{
    /// <summary>
    /// Class representing method for writing files into a directory.
    /// </summary>
    public class RoiWriter
    {
        /// <summary>
        /// Writes list of doubles into a png file.
        /// </summary>
        /// <param name="roidataset">The prototyp.</param>
        /// <param name="path">The path.</param>
        public void RoiWriterTool(Roi roidataset, string path)
        {
            var roiConverter = new RoiConverter();

            using (var bitmap = roiConverter.RoiToBitmap(roidataset))
            {
                var writepath = Path.GetFullPath(Path.Combine(path, roidataset.Name));

                bitmap.Save(writepath + ".png", ImageFormat.Png);
            }
        }
    }
}
