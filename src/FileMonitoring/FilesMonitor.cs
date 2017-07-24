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
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace rideaway_backend.FileMonitoring
{
    /// <summary>
    /// A file monitor monitoring all files relevant to one instance.
    /// </summary>
    public class FilesMonitor<T> : IFilesMonitor
    {
        private readonly int _intervalInMillis;
        private readonly List<FileMonitor> _filesToMonitor; // The list of files to monitor.
        private bool _hasChanged; // Holds the has changed flag.
        private long _lastChange; // Holds the last change.
        private readonly Timer _timer; // Holds the timer.
        private readonly Func<T, bool> _trigger; // Called when file(s) have changed.
        private readonly T _param; // Holds the parameters to pass to the trigger.
        private readonly object _sync = new object(); // Holds the sync object.

        /// <summary>
        /// Creates a new instance monitor.
        /// </summary>
        public FilesMonitor(Func<T, bool> trigger, T param, int intervalInMillis = 60 * 1000)
        {
            _filesToMonitor = new List<FileMonitor>();
            _hasChanged = false;
            _lastChange = DateTime.Now.Ticks;
            _trigger = trigger;
            _param = param;
            _intervalInMillis = intervalInMillis;

            _timer = new Timer(Tick, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Starts to monitor.
        /// </summary>
        public void Start()
        {
            _hasChanged = false;
            _lastChange = DateTime.Now.Ticks;
            _timer.Change(_intervalInMillis, _intervalInMillis);
            foreach (var monitor in _filesToMonitor)
            {
                monitor.FileChanged += monitor_FileChanged;
                monitor.Start();
            }
        }

        
        /// <summary>
        /// Called when one for the file monitors detects a change.
        /// </summary>
        /// <param name="sender"></param>
        void monitor_FileChanged(FileMonitor sender)
        {
            lock (_sync)
            {
                _hasChanged = true;
                _lastChange = DateTime.Now.Ticks;
            }
        }

        /// <summary>
        /// Stops monitoring.
        /// </summary>
        public void Stop()
        {
            foreach (var monitor in _filesToMonitor)
            {
                monitor.Stop();
                // ReSharper disable once DelegateSubtraction 
                // It is okay here: http://stackoverflow.com/questions/11180068/delegate-subtraction-has-unpredictable-result-in-resharper-c
                monitor.FileChanged -= monitor_FileChanged;
            }
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Adds a new file to monitor.
        /// </summary>
        /// <param name="path"></param>
        public void AddFile(string path)
        {
            if (File.Exists(path))
            {
                _filesToMonitor.Add(new FileMonitor(path));
            }
        }

        /// <summary>
        /// Called when the timer ticks.
        /// </summary>
        private void Tick(object state)
        {
            lock (_sync)
            {
                if (_hasChanged)
                {
                    var timeSpan = new TimeSpan(DateTime.Now.Ticks - _lastChange);
                    if (timeSpan.TotalMilliseconds > (float)_intervalInMillis / 2)
                    { // more than 4 mins ago when the last change was reported.
                        bool isAvailable = true;
                        foreach (var monitor in _filesToMonitor)
                        {
                            if (!monitor.IsAvailable())
                            {
                                _hasChanged = true;
                                _lastChange = DateTime.Now.Ticks;
                                isAvailable = false;
                                break;
                            }
                        }
                        if (isAvailable)
                        { // call reload.
                            _hasChanged = false;
                            if (!_trigger(_param))
                            { // loading failed, try again in 5 mins.
                                _hasChanged = true;
                                _lastChange = DateTime.Now.Ticks;
                            }
                        }
                    }
                }
            }
        }
    }
}