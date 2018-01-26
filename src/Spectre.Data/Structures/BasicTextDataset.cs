/*
 * BasicTextDataset.cs
 * Class representing dataset created from streaming an ordinary text
 * file containing formatted data.
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
using System.IO;
using System.Linq;

namespace Spectre.Data.Structures
{
    /// <inheritdoc />
    /// <summary>
    ///     Class representing dataset created from streaming an ordinary text file containing formatted data.
    /// </summary>
    public class BasicTextDataset : IDataset
    {
        #region Fields

        /// <summary>
        ///     Container for storing spatial coordinates for every loaded spectrum.
        /// </summary>
        private readonly int[,] _spatialCoordinates;

        /// <summary>
        ///     Array of m/z values for all the spectras.
        /// </summary>
        private readonly double[] _mz;

        /// <summary>
        ///     Container for storing intensity values for every loaded spectrum.
        /// </summary>
        private readonly double[,] _intensityArray;
        
        #endregion

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="BasicTextDataset" /> class.
        ///     Constructor with raw value initialization.
        /// </summary>
        /// <param name="mz">Array of m/z values.</param>
        /// <param name="data">Array of intensity values.</param>
        /// <param name="coordinates">Array of spatial coordinates.</param>
        public BasicTextDataset(double[] mz, double[,] data, int[,] coordinates)
        {
            if ((mz == null) || (data == null))
            {
                throw new InvalidDataException(message: "The input data is null.");
            }
            if ((coordinates != null) || (coordinates.GetLength(dimension: 0) != data.GetLength(dimension: 0)))
            {
                throw new InvalidDataException(
                    message: "Amount of input spectra does not match the amount of spatial coordinates.");
            }

            if (coordinates.GetLength(1) != 3)
            {
                throw new InvalidDataException("Coordinates must be 3D.");
            }
            if (mz.Length != data.GetLength(dimension: 1))
            {
                throw new InvalidDataException(message: "Length of the data must be equal to length of m/z values.");
            }

            _mz = mz;
            _intensityArray = data;
            _spatialCoordinates = coordinates;
        }

        #endregion

        #region Properties

        /// <inheritdoc cref="IDataset"/>
        public Metadata Metadata => throw new NotImplementedException("Metadata is not supported yet.");

        /// <summary>
        ///     See <see cref="IDataset" /> for description.
        /// </summary>
        public IEnumerable<SpatialCoordinates> SpatialCoordinates => Enumerable
            .Range(0, _spatialCoordinates.GetLength(0))
            .Select(GetSpatialCoordinates);
        
        /// <summary>
        ///     See <see cref="IDataset" /> for description.
        /// </summary>
        public int SpectrumLength => _mz.Length;

        /// <summary>
        ///     See <see cref="IDataset" /> for description.
        /// </summary>
        public int SpectrumCount => _intensityArray.GetLength(0);

        #endregion

        #region IDataset Methods
        
        /// <inheritdoc cref="IDataset"/>
        public SpatialCoordinates GetSpatialCoordinates(int spectrumIdx) => new SpatialCoordinates(
            _spatialCoordinates[spectrumIdx, 0],
            _spatialCoordinates[spectrumIdx, 1],
            _spatialCoordinates[spectrumIdx, 2]
        );

        /// <inheritdoc cref="IDataset"/>
        public double[] GetRawMzArray() => _mz;

        /// <inheritdoc cref="IDataset"/>
        public double[] GetRawIntensityArray(int spectrumIdx) => Enumerable
            .Range(0, _intensityArray.GetLength(1))
            .Select(i => _intensityArray[spectrumIdx, i])
            .ToArray();

        /// <inheritdoc cref="IDataset"/>
        public double[] GetRawIntensityRow(int valueIdx) => Enumerable
            .Range(0, _intensityArray.GetLength(0))
            .Select(i => _intensityArray[i, valueIdx])
            .ToArray();

        /// <inheritdoc cref="IDataset"/>
        public double[,] GetRawIntensities() => _intensityArray;

        /// <inheritdoc cref="IDataset"/>
        public int[,] GetRawSpacialCoordinates() => _spatialCoordinates;

        #endregion
    }
}
