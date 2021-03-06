﻿//The MIT License (MIT)

//Copyright (c) 2017 SharpSoftware

//Permission is hereby granted, free of charge, to any person obtaining a copy 
//of this software and associated documentation files (the "Software"), to deal 
//in the Software without restriction, including without limitation the rights 
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
//copies of the Software, and to permit persons to whom the Software is 
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included 
//in all copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
//THE SOFTWARE.

using System;
using System.IO;
using System.Threading;

namespace rideaway_backend.FileMonitoring
{
    /// <summary>
    /// Represents a file monitor that continually monitors a file for changes.
    /// </summary>
    internal class FileMonitor
    {
        private const int MonitorInterval = 9 * 1000;
        private readonly FileInfo _fileInfo; // Holds the fileinfo of the file to monitor.
        private readonly Timer _timer; // Holds the timer to poll file changes.

        /// <summary>
        /// Creates a new file monitor for the given file.
        /// </summary>
        /// <param name="path"></param>
        public FileMonitor(string path)
        {
            _fileInfo = new FileInfo(path);
            _timestamp = _fileInfo.LastWriteTime.Ticks;

            _timer = new Timer(Tick, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Starts monitoring.
        /// </summary>
        public void Start()
        {
            _fileInfo.Refresh();
            _timestamp = _fileInfo.LastWriteTime.Ticks;
            _timer.Change(MonitorInterval, MonitorInterval);
        }

        /// <summary>
        /// Stops monitoring.
        /// </summary>
        public void Stop()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Returns true if the file is available for reading and not in a state that is changing.
        /// </summary>
        /// <returns></returns>
        public bool IsAvailable()
        {
            lock (_sync)
            {
                _fileInfo.Refresh();
                if (!_fileInfo.Exists)
                {
                    return false;
                }
                return !IsFileLocked(_fileInfo);
            }
        }

        /// <summary>
        /// An action to report that the file has changed and is accessible.
        /// </summary>
        public Action<FileMonitor> FileChanged
        {
            get;
            set;
        }

        private long _timestamp; // Holds the last modified timestamp.
        private readonly object _sync = new object(); // Holds an object that is used to sync the timer.

        /// <summary>
        /// Called when the timer ticks.
        /// </summary>
        /// <param name="state"></param>
        private void Tick(object state)
        {
            lock (_sync)
            {
                if (FileChanged != null)
                {
                    _fileInfo.Refresh();
                    if (_fileInfo.Exists)
                    {
                        if (_timestamp != _fileInfo.LastWriteTime.Ticks)
                        { // file has been written to.
                            _timestamp = _fileInfo.LastWriteTime.Ticks;
                            FileChanged(this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Not so nice code to check if a file is locked.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read);
            }
            catch
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                stream?.Dispose();
            }

            //file is not locked
            return false;
        }
    }
}