﻿namespace DuetAPI.ObjectModel
{
    /// <summary>
    /// Information about a CNC spindle
    /// </summary>
    public sealed class Spindle : ModelObject
    {
        /// <summary>
        /// Active RPM
        /// </summary>
        public int Active
        {
            get => _active;
			set => SetPropertyValue(ref _active, value);
        }
        private int _active;
        
        /// <summary>
        /// Current RPM, negative if anticlockwise direction
        /// </summary>
        public int Current
        {
            get => _current;
			set => SetPropertyValue(ref _current, value);
        }
        private int _current;

        /// <summary>
        /// Frequency (in Hz)
        /// </summary>
        public int Frequency
        {
            get => _frequency;
			set => SetPropertyValue(ref _frequency, value);
        }
        private int _frequency;

        /// <summary>
        /// Minimum RPM when turned on
        /// </summary>
        public int Min
        {
            get => _min;
			set => SetPropertyValue(ref _min, value);
        }
        private int _min = 60;

        /// <summary>
        /// Maximum RPM
        /// </summary>
        public int Max
        {
            get => _max;
			set => SetPropertyValue(ref _max, value);
        }
        private int _max = 10000;

        /// <summary>
        /// Mapped tool number or -1 if not assigned
        /// </summary>
        public int Tool
        {
            get => _tool;
			set => SetPropertyValue(ref _tool, value);
        }
        private int _tool = -1;
    }
}