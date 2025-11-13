namespace Rc41.Core.Interfaces
{
    /// <summary>
    /// Interface for printer configuration queries
    /// </summary>
    public interface IPrinterAdapter
    {
        /// <summary>
        /// Get the current printer mode (M, T, N)
        /// </summary>
        char PrinterMode();

        /// <summary>
        /// Check if printer is powered on
        /// </summary>
        bool PrinterOn();
    }
}
