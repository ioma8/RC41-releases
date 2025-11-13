namespace Rc41.Core.Interfaces
{
    /// <summary>
    /// Interface for timer control operations
    /// </summary>
    public interface ITimerAdapter
    {
        /// <summary>
        /// Enable or disable the display timer
        /// </summary>
        void DisplayTimerEnabled(bool enabled);

        /// <summary>
        /// Enable or disable the run timer
        /// </summary>
        void RunTimerEnabled(bool enabled);

        /// <summary>
        /// Get whether fast mode is enabled
        /// </summary>
        bool Fast();

        /// <summary>
        /// Access to the display timer for interval changes
        /// </summary>
        System.Windows.Forms.Timer DisplayTimer { get; }
    }
}
