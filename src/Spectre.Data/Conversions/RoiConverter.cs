/*
 * RoiConverter.cs
 * Class containing BitmapToRoi and RoiToBitmap methods.

   Copyright 2018 Roman Lisak, Grzegorz Mrukwa

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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Brushes;
using Spectre.Data.Structures;

namespace Spectre.Data.Conversions
{
    /// <summary>
    /// Class containing BitmapToRoi and RoiToBitmap methods.
    /// </summary>
    public class RoiConverter
    {
        private static readonly Rgba32 RoiColor = Rgba32.Black;
        private static readonly Rgba32 DefaultColor = Rgba32.White;

        /// <summary>
        /// Bitmap to roi converter.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <param name="name">The name.</param>
        /// <returns>
        /// Roi dataset.
        /// </returns>
        public Roi BitmapToRoi(Image<Rgba32> bitmap, string name)
        {
            var roidataset = new Roi(
                name,
                (uint)bitmap.Width,
                (uint)bitmap.Height,
                new List<RoiPixel>());

            for (int xcoordinate = 0; xcoordinate < bitmap.Width; xcoordinate++)
            {
                for (int ycoordinate = 0; ycoordinate < bitmap.Height; ycoordinate++)
                {
                    var color = bitmap[xcoordinate, ycoordinate];
                    if (color.Equals(RoiColor))
                    {
                        roidataset.Pixels.Add(new RoiPixel(xcoordinate, ycoordinate));
                    }
                }
            }

            return roidataset;
        }

        /// <summary>
        /// Roi to bitmap converter.
        /// </summary>
        /// <param name="roi">The roi.</param>
        /// <returns>
        /// Bitmap.
        /// </returns>
        public Image<Rgba32> RoiToBitmap(Roi roi)
        {
            var bitmap = new Image<Rgba32>((int)roi.Width, (int)roi.Height);
            var backgroundBrush = new SolidBrush<Rgba32>(DefaultColor);
            bitmap.Mutate(context => context.Fill(backgroundBrush));
            
            foreach (var pixel in roi.Pixels)
            {
                bitmap[pixel.XCoordinate, pixel.YCoordinate] = RoiColor;
            }

            return bitmap;
        }
    }
}
