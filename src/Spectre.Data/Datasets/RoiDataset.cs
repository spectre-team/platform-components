﻿/*
 * RoiDataset.cs
 * Class represeting dataset with data of regions of interest.

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectre.Data.Datasets
{
    /// <summary>
    /// Dataset for regions of interest.
    /// </summary>
    public class RoiDataset
    {
        /// <summary>
        /// Gets or sets the x coordinates.
        /// </summary>
        /// <value>
        /// The x coordinates.
        /// </value>
        public List<double> XCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the y coordinates.
        /// </summary>
        /// <value>
        /// The y coordinates.
        /// </value>
        public List<double> YCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the name of the roi.
        /// </summary>
        /// <value>
        /// The name of the roi.
        /// </value>
        public string RoiName { get; set; }
    }
}