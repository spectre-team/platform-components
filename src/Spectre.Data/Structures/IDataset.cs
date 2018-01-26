/*
 * IDataset.cs
 * Contains interface class for basic functionalities of dataset
 * representing measurements from single sample point
 *
   Copyright 2018 Dariusz Kuchta, Grzegorz Mrukwa

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

namespace Spectre.Data.Structures
{
    /// <summary>
    /// Contains interface for basic functionalities of dataset representing measurements from single sample point.
    /// </summary>
    public interface IDataset
    {
        #region Metadata

        /// <summary>
        /// Property containing metadata of the dataset.
        /// </summary>
        Metadata Metadata { get; }

        /// <summary>
        /// Property for storing spacial coordinates for spectras in dataset.
        /// </summary>
        IEnumerable<SpatialCoordinates> SpatialCoordinates { get; }

        /// <summary>
        /// Property containing number of values in a single spectrum.
        /// </summary>
        int SpectrumLength { get; }

        /// <summary>
        /// Property containing amount of spectra existing in dataset.
        /// </summary>
        int SpectrumCount { get; }

        #endregion

        #region Data access

        /// <summary>
        /// Method returning spatial coordinates of given spectrum.
        /// </summary>
        /// <param name="spectrumIdx">Index of spectrum.</param>
        /// <returns>Spatial coordinates of given spectrum.</returns>
        SpatialCoordinates GetSpatialCoordinates(int spectrumIdx);
        
        /// <summary>
        /// Getter for whole array of raw m/z values used in dataset.
        /// </summary>
        /// <returns>Array of all m/z values.</returns>
        double[] GetRawMzArray();

        /// <summary>
        /// Get intensities for given spectrum.
        /// </summary>
        /// <param name="spectrumIdx">Index of spectrum.</param>
        /// <returns>Array of intensities in specified spectrum.</returns>
        double[] GetRawIntensityArray(int spectrumIdx);

        /// <summary>
        /// Get heatmap values for given mass channel.
        /// </summary>
        /// <param name="valueIdx">Index of value.</param>
        /// <returns>Array of intensity values from all spectra at given mass channel.</returns>
        double[] GetRawIntensityRow(int valueIdx);

        /// <summary>
        /// Getter for all the intensity values from all spectra from dataset.
        /// </summary>
        /// <returns>Multidimensional array of all the intensities present in dataset.</returns>
        double[,] GetRawIntensities();

        /// <summary>
        /// Getter for spatial coordinates of all spectra present in dataset.
        /// </summary>
        /// <returns>Multidimensional array of spatial coordinates.</returns>
        int[,] GetRawSpacialCoordinates();

        #endregion
    }
}
