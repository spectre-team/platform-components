﻿/*
 * Roi.cs
 * Class represents a region over an image.

   Copyright 2018 Roman Lisak

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
using System.Linq;

namespace Spectre.Data.Structures
{
    /// <summary>
    /// Dataset containing information about a single roi.
    /// </summary>
    public class Roi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Roi" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="pixels">The roipixel.</param>
        public Roi(string name, uint width, uint height, IList<RoiPixel> pixels)
        {
            Name = name;
            Width = width;
            Height = height;

            if (pixels.Any(r => r.XCoordinate > width) || pixels.Any(r => r.YCoordinate > height))
            {
                throw new ArgumentOutOfRangeException("Given roi pixels cannot exceed specified dimensions.");
            }

            Pixels = pixels;
        }

        /// <summary>
        /// Gets the roi pixels.
        /// </summary>
        /// <value>
        /// The roi pixels.
        /// </value>
        public IList<RoiPixel> Pixels { get; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        public uint Width { get; }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        public uint Height { get; }
    }
}