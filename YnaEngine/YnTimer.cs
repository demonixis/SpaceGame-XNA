// YnaEngine - Copyright (C) YnaEngine team
// This file is subject to the terms and conditions defined in
// file 'LICENSE', which is part of this source code package.
using System;
using Microsoft.Xna.Framework;

namespace Yna.Engine
{
    /// <summary>
    /// A timer class
    /// </summary>
    public class YnTimer : YnBasicEntity
    {
        #region Private declarations

        private int _interval;
        private int _repeat;
        private int _counter;
        private long _elapsedTime;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the Duration of the timer
        /// </summary>
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        /// <summary>
        /// Gets or sets the number of time the timer is repeated
        /// </summary>
        public int Repeat
        {
            get { return _repeat; }
            set { _repeat = value; }
        }

        /// <summary>
        /// Get elapsed time since the start of the timer
        /// </summary>
        public long ElapsedTime
        {
            get { return _elapsedTime; }
        }


        /// <summary>
        /// Get the time beforce the timer is completed
        /// </summary>
        public long TimeRemaining
        {
            get
            {
                if (_elapsedTime == 0)
                    return 0;
                else
                    return (long)(Interval - ElapsedTime);
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Triggered when the timer is terminated and restart
        /// </summary>
        public event Action ReStarted = null;

        /// <summary>
        /// Triggered when the timer is finish
        /// </summary>
        public event Action Completed = null;

        #endregion

        /// <summary>
        /// Create a new Timer with an interval
        /// </summary>
        /// <param name="interval">Time (ms) interval between each trigger</param>
        public YnTimer(int interval)
        {
            Interval = interval;
            Repeat = 0;
            _elapsedTime = 0;
            _counter = Repeat;
            _enabled = false;
        }

        /// <summary>
        /// Create a new Timer with an interval value and repeat factor
        /// </summary>
        /// <param name="interval">Time (ms) interval between each trigger</param>
        /// <param name="repeat">Repeat count</param>
        public YnTimer(int interval, int repeat)
            : this (interval)
        {
            Repeat = repeat;
        }

        /// <summary>
        /// Start the timer
        /// </summary>
        public void Start()
        {
            _enabled = true;
            TimerRestarted();
        }

        /// <summary>
        /// Restart the timer with rested values passed to the contructor
        /// </summary>
        public void Restart()
        {
            _enabled = true;
            _counter = 0;
            _elapsedTime = 0;
            TimerRestarted();
        }

        /// <summary>
        /// Pause the timer 
        /// </summary>
        public void Pause()
        {
            _enabled = false;
        }

        /// <summary>
        /// Resume the timer avec a call of Pause() method
        /// </summary>
        public void Resume()
        {
            _enabled = true;
        }

        /// <summary>
        /// Stop the timer
        /// </summary>
        public void Stop()
        {
            _enabled = false;
            _counter = 0;
            _elapsedTime = 0;
        }

        /// <summary>
        /// Kill the timer and shutdown all events
        /// </summary>
        public void Kill()
        {
            _enabled = false;
            _counter = 0;
            _elapsedTime = 0;
        }

        #region Events handlers

        private void TimerRestarted() => ReStarted?.Invoke();

        private void TimerCompleted() => Completed?.Invoke();

        #endregion

        /// <summary>
        /// Updating the timer
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (_enabled)
            {
                _elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (_elapsedTime >= Interval)
                {
                    if (_counter == 0)
                    {
                        _enabled = false;
                    }
                    else
                    {
                        _counter--;
                        TimerRestarted();
                    }

                    TimerCompleted();
                    _elapsedTime = 0;
                }
            }
        }
    }
}
