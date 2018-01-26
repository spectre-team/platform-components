/*
 * RoiConverter.cs
 * Class containing BitmapToRoi and RoiToBitmap methods.

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
using System.Drawing;
using Spectre.Data.Deprecated.Datasets;

namespace Spectre.Data.Deprecated.RoiIo
{
    /// <summary>
    /// Class containing BitmapToRoi and RoiToBitmap methods.
    /// </summary>
    public class RoiConverter : IRoiConverter
    {
        /// <summary>
        /// Bitmap to roi converter.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Roi dataset.
        /// </returns>
        public Roi BitmapToRoi(Bitmap bitmap, string name)
        {
            var color = default(Color);
            var roiColor = 0;

            var roidataset = new Roi(
                name,
                (uint)bitmap.Width,
                (uint)bitmap.Height,
                new List<RoiPixel>());

            for (int xcoordinate = 0; xcoordinate < bitmap.Width; xcoordinate++)
            {
                for (int ycoordinate = 0; ycoordinate < bitmap.Height; ycoordinate++)
                {
                    color = bitmap.GetPixel(xcoordinate, ycoordinate);
                    if (color.B == roiColor)
                    {
                        roidataset.RoiPixels.Add(new RoiPixel(xcoordinate, ycoordinate));
                    }
                }
            }

            return roidataset;
        }

        /// <summary>
        /// Roi to bitmap converter.
        /// </summary>
        /// <param name="roidataset">The roidataset.</param>
        /// <returns>
        /// Bitmap.
        /// </returns>
        public Bitmap RoiToBitmap(Roi roidataset)
        {
            var bitmap = new Bitmap((int)roidataset.Width, (int)roidataset.Height);
            Color backgroundColor = Color.White;

            var graphicsobject = Graphics.FromImage(bitmap);
            graphicsobject.Clear(backgroundColor);

            for (int listiterator = 0; listiterator < roidataset.RoiPixels.Count; listiterator++)
            {
                bitmap.SetPixel(
                    roidataset.RoiPixels[listiterator].XCoordinate,
                    roidataset.RoiPixels[listiterator].YCoordinate,
                    Color.Black);
            }

            return bitmap;
        }
    }
}
