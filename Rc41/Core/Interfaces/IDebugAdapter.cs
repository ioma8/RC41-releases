namespace Rc41.Core.Interfaces
{
    /// <summary>
    /// Interface for debug and trace operations
    /// </summary>
    public interface IDebugAdapter
    {
        /// <summary>
        /// Check if trace mode is enabled
        /// </summary>
        bool Trace();

        /// <summary>
        /// Output a trace message
        /// </summary>
        void Trace(string message);

        /// <summary>
        /// Trace registers (output debug information)
        /// </summary>
        void TraceRegs();

        /// <summary>
        /// Print debug output
        /// </summary>
        void DebugPrint(string line);
    }
}
