using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace FreeScoreBoard.Core.Timer
{
    /// <summary>
    /// Defines constants for the multimedia Timer's event types.
    /// </summary>
    public enum TimerMode
    {
        /// <summary>
        /// Timer event occurs once.
        /// </summary>
        OneShot,

        /// <summary>
        /// Timer event occurs periodically.
        /// </summary>
        Periodic
    }

    /// <summary>
    /// Represents information about the multimedia Timer's capabilities.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct TimerCaps
    {
        /// <summary>
        /// Minimum supported period in milliseconds.
        /// </summary>
        public int periodMin;

        /// <summary>
        /// Maximum supported period in milliseconds.
        /// </summary>
        public int periodMax;
    }

    /// <summary>
    /// Represents the Windows multimedia timer.
    /// </summary>
    /// 
    public class ClsTimer : IComponent
    {
        #region Timer Members

        #region Delegates

        // Represents the method that is called by Windows when a timer event occurs.
        private delegate void TimeProc(int id, int msg, int user, int param1, int param2);

        // Represents methods that raise events.
        private delegate void EventRaiser(EventArgs e);

        #endregion

        #region Win32 Multimedia Timer Functions

        // Gets timer capabilities.
        [DllImport("winmm.dll")]
        private static extern int timeGetDevCaps(ref TimerCaps caps, int sizeOfTimerCaps);

        // Creates and starts the timer.
        [DllImport("winmm.dll")]
        private static extern int timeSetEvent(int delay, int resolution, TimeProc proc, int user, int mode);

        // Stops and destroys the timer.
        [DllImport("winmm.dll")]
        private static extern int timeKillEvent(int id);

        // Indicates that the operation was successful.
        private const int TIMERR_NOERROR = 0;

        #endregion

        #region Fields

        // Timer identifier.
        private int timerID;

        // Timer mode.
        private volatile TimerMode mode;

        // Period between timer events in milliseconds.
        private volatile int period;

        // Timer resolution in milliseconds.
        private volatile int resolution;

        // Called by Windows when a timer periodic event occurs.
        private TimeProc timeProcPeriodic;

        // Called by Windows when a timer one shot event occurs.
        private TimeProc timeProcOneShot;

        // Represents the method that raises the Tick event.
        private EventRaiser tickRaiser;

        // Indicates whether or not the timer is running.
        private bool running;

        // Indicates whether or not the timer has been disposed.
        private volatile bool disposed;

        // The ISynchronizeInvoke object to use for marshaling events.
        private ISynchronizeInvoke synchronizingObject;

        // For implementing IComponent.
        private ISite site;

        // Multimedia timer capabilities.
        private static TimerCaps mycaps;

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the Timer has started;
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Occurs when the Timer has stopped;
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Occurs when the time period has elapsed.
        /// </summary>
        public event EventHandler Tick;

        #endregion

        #region Construction

        /// <summary>
        /// Initialize class.
        /// </summary>
        static ClsTimer()
        {
            // Get multimedia timer capabilities.
            timeGetDevCaps(ref mycaps, Marshal.SizeOf(mycaps));
        }

        /// <summary>
        /// Initializes a new instance of the Timer class with the specified IContainer.
        /// </summary>
        /// <param name="container">
        /// The IContainer to which the Timer will add itself.
        /// </param>
        public ClsTimer(IContainer container)
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);

            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the Timer class.
        /// </summary>
        public ClsTimer()
        {
            this.Initialize();
        }

        ~ClsTimer()
        {
            if (this.IsRunning)
            {
                // Stop and destroy timer.
                timeKillEvent(this.timerID);
            }
        }

        // Initialize timer with default values.
        private void Initialize()
        {
            this.mode = TimerMode.Periodic;
            this.period = Capabilities.periodMin;
            this.resolution = 1;

            this.running = false;

            this.timeProcPeriodic = new TimeProc(this.TimerPeriodicEventCallback);
            this.timeProcOneShot = new TimeProc(this.TimerOneShotEventCallback);
            this.tickRaiser = new EventRaiser(this.OnTick);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the timer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// The timer has already been disposed.
        /// </exception>
        /// <exception cref="TimerStartException">
        /// The timer failed to start.
        /// </exception>
        public void Start()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("Timer");
            }

            if (this.IsRunning)
            {
                return;
            }

            // If the periodic event callback should be used.
            if (this.Mode == TimerMode.Periodic)
            {
                // Create and start timer.
                this.timerID = timeSetEvent(this.Period, this.Resolution, this.timeProcPeriodic, 0, (int)this.Mode);
            }
            else
            {
                // Else the one shot event callback should be used.
                // Create and start timer.
                this.timerID = timeSetEvent(this.Period, this.Resolution, this.timeProcOneShot, 0, (int)this.Mode);
            }

            // If the timer was created successfully.
            if (this.timerID != 0)
            {
                this.running = true;

                if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
                {
                    this.SynchronizingObject.BeginInvoke(
                        new EventRaiser(this.OnStarted),
                        new object[] { EventArgs.Empty });
                }
                else
                {
                    this.OnStarted(EventArgs.Empty);
                }
            }
            else
            {
                throw new TimerStartException("Unable to start multimedia Timer.");
            }
        }

        /// <summary>
        /// Stops timer.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>
        public void Stop()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException("Timer");
            }

            if (!this.running)
            {
                return;
            }

            // Stop and destroy timer.
            int result = timeKillEvent(this.timerID);

            Debug.Assert(result == TIMERR_NOERROR);

            this.running = false;

            if (this.SynchronizingObject != null && this.SynchronizingObject.InvokeRequired)
            {
                this.SynchronizingObject.BeginInvoke(
                    new EventRaiser(this.OnStopped),
                    new object[] { EventArgs.Empty });
            }
            else
            {
                this.OnStopped(EventArgs.Empty);
            }
        }

        #region Callbacks

        // Callback method called by the Win32 multimedia timer when a timer
        // periodic event occurs.
        private void TimerPeriodicEventCallback(int id, int msg, int user, int param1, int param2)
        {
            if (this.synchronizingObject != null)
            {
                this.synchronizingObject.BeginInvoke(this.tickRaiser, new object[] { EventArgs.Empty });
            }
            else
            {
                this.OnTick(EventArgs.Empty);
            }
        }

        // Callback method called by the Win32 multimedia timer when a timer
        // one shot event occurs.
        private void TimerOneShotEventCallback(int id, int msg, int user, int param1, int param2)
        {
            if (this.synchronizingObject != null)
            {
                this.synchronizingObject.BeginInvoke(this.tickRaiser, new object[] { EventArgs.Empty });
                this.Stop();
            }
            else
            {
                this.OnTick(EventArgs.Empty);
                this.Stop();
            }
        }

        #endregion

        #region Event Raiser Methods

        // Raises the Disposed event.
        private void OnDisposed(EventArgs e)
        {
            EventHandler handler = this.Disposed;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        // Raises the Started event.
        private void OnStarted(EventArgs e)
        {
            EventHandler handler = this.Started;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        // Raises the Stopped event.
        private void OnStopped(EventArgs e)
        {
            EventHandler handler = this.Stopped;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        // Raises the Tick event.
        private void OnTick(EventArgs e)
        {
            EventHandler handler = this.Tick;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion        

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the object used to marshal event-handler calls.
        /// </summary>
        public ISynchronizeInvoke SynchronizingObject
        {
            get
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                return this.synchronizingObject;
            }

            set
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                this.synchronizingObject = value;
            }
        }

        /// <summary>
        /// Gets or sets the time between Tick events.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>   
        public int Period
        {
            get
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                return this.period;
            }

            set
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }
                else if (value < Capabilities.periodMin || value > Capabilities.periodMax)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        value,
                        "Multimedia Timer period out of range.");
                }

                this.period = value;

                if (this.IsRunning)
                {
                    this.Stop();
                    this.Start();
                }
            }
        }

        /// <summary>
        /// Gets or sets the timer resolution.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>        
        /// <remarks>
        /// The resolution is in milliseconds. The resolution increases 
        /// with smaller values; a resolution of 0 indicates periodic events 
        /// should occur with the greatest possible accuracy. To reduce system 
        /// overhead, however, you should use the maximum value appropriate 
        /// for your application.
        /// </remarks>
        public int Resolution
        {
            get
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                return this.resolution;
            }

            set
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }
                else if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        value,
                        "Multimedia timer resolution out of range.");
                }

                this.resolution = value;

                if (this.IsRunning)
                {
                    this.Stop();
                    this.Start();
                }
            }
        }

        /// <summary>
        /// Gets the timer mode.
        /// </summary>
        /// <exception cref="ObjectDisposedException">
        /// If the timer has already been disposed.
        /// </exception>
        public TimerMode Mode
        {
            get
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                return this.mode;
            }

            set
            {
                if (this.disposed)
                {
                    throw new ObjectDisposedException("Timer");
                }

                this.mode = value;

                if (this.IsRunning)
                {
                    this.Stop();
                    this.Start();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the Timer is running.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                return this.running;
            }
        }

        /// <summary>
        /// Gets the timer capabilities.
        /// </summary>
        public static TimerCaps Capabilities
        {
            get
            {
                return mycaps;
            }
        }

        #endregion

        #endregion

        #region IComponent Members

        public event System.EventHandler Disposed;

        public ISite Site
        {
            get
            {
                return this.site;
            }

            set
            {
                this.site = value;
            }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Frees timer resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                if (this.disposed)
                {
                    return;
                }

                if (this.IsRunning)
                {
                    this.Stop();
                }

                this.disposed = true;

                this.OnDisposed(EventArgs.Empty);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion       
    }

    /// <summary>
    /// The exception that is thrown when a timer fails to start.
    /// </summary>
    public class TimerStartException : ApplicationException
    {
        /// <summary>
        /// Initializes a new instance of the TimerStartException class.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception. 
        /// </param>
        public TimerStartException(string message) : base(message)
        {
        }
    }

}
