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
        bool IsTraceEnabled();

        /// <summary>
        /// Output a trace message
        /// </summary>
        /// <param name="message">The trace message to output</param>
        void TraceMessage(string message);

        /// <summary>
        /// Trace registers (output debug information)
        /// </summary>
        void TraceRegs();

        /// <summary>
        /// Print debug output
        /// </summary>
        /// <param name="line">The debug line to print</param>
        void DebugPrint(string line);
    }
}
