﻿/*
 * TooSmallUpdateIntervalException.cs
 * Exception thrown on unreasonably small update interval.
 *
   Copyright 2017 Grzegorz Mrukwa

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

namespace Spectre.Service.Deprecated
{
    /// <summary>
    /// Thrown when update interval is unreasonably small, to avoid congestion.
    /// </summary>
    /// <seealso cref="System.ArgumentOutOfRangeException" />
    public class TooSmallUpdateIntervalException : ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TooSmallUpdateIntervalException"/> class.
        /// </summary>
        /// <param name="updateInterval">The update interval.</param>
        public TooSmallUpdateIntervalException(double updateInterval)
        {
            UpdateInterval = updateInterval;
        }

        /// <summary>
        /// Gets the update interval.
        /// </summary>
        /// <value>
        /// The update interval.
        /// </value>
        public double UpdateInterval { get; }
    }
}
