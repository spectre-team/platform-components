﻿/*
 * ConsoleCaptureService.cs
 * Implementation of console capture.
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
using System.IO;
using System.Text;
using System.Threading;
using Spectre.Service.Deprecated.Abstract;
using Timer = System.Timers.Timer;

namespace Spectre.Service.Deprecated
{
    /// <summary>
    /// Captures console output.
    /// </summary>
    /// <seealso cref="IConsoleCaptureService" />
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Assert, Unrestricted = true)]
    public class ConsoleCaptureService : IConsoleCaptureService
    {
        #region Fields

        /// <summary>
        /// Minimal interval which should suffice for updates without scheduling too much jobs.
        /// </summary>
        public const double MinimalReasonableUpdateInterval = 50;

        /// <summary>
        /// The internal writer.
        /// </summary>
        private readonly StringWriter _writer;

        /// <summary>
        /// The global stdout.
        /// </summary>
        private readonly TextWriter _stdout;

        /// <summary>
        /// The timer for update notification.
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// Timer update interval.
        /// </summary>
        private readonly double _updateInterval;

        /// <summary>
        /// If true, instance is useless.
        /// </summary>
        private bool _disposed;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleCaptureService"/> class.
        /// </summary>
        /// <param name="updateInterval">The update interval.</param>
        /// <exception cref="TooSmallUpdateIntervalException">updateInterval lower than MinimalReasonableUpdateInterval</exception>
        public ConsoleCaptureService(double updateInterval)
        {
            if (updateInterval < ConsoleCaptureService.MinimalReasonableUpdateInterval)
            {
                throw new TooSmallUpdateIntervalException(updateInterval);
            }
            _stdout = Console.Out;
            var builder = new StringBuilder();
            _writer = new StringWriter(builder);
            Console.SetOut(_writer);
            _updateInterval = updateInterval;
            _timer = new Timer(_updateInterval);
            Content = string.Empty;
            _timer.Elapsed += (sender, args) =>
            {
#pragma warning disable SA1305 // Field names must not use Hungarian notation
                var upToDateContent = builder.ToString();
#pragma warning restore SA1305 // Field names must not use Hungarian notation
                var suffix = builder.ToString(Content.Length, length: upToDateContent.Length - Content.Length);
                if (Content != upToDateContent)
                {
                    Content = upToDateContent;
                    OnWritten(suffix);
                }
            };
            _timer.Start();
        }

        #endregion

        #region IConsoleCaptureService

        /// <summary>
        /// Occurs when anything was written.
        /// </summary>
        public event EventHandler<string> Written;

        /// <summary>
        /// Gets the captured content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        public string Content { get; private set; }

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(obj: this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            Thread.Sleep(millisecondsTimeout: (int)_updateInterval + 1);

            Console.SetOut(_stdout);
            if (disposing)
            {
                _writer.Dispose();
                _timer.Stop();
                _timer.Dispose();
            }
            _disposed = true;
        }

        #endregion

        #region OnWritten

        /// <summary>
        /// Called when console was written.
        /// </summary>
        /// <param name="entry">The entry.</param>
        protected virtual void OnWritten(string entry)
        {
            Written?.Invoke(sender: this, e: entry);
        }

        #endregion
    }
}
