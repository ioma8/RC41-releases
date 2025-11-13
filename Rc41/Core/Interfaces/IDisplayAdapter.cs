namespace Rc41.Core.Interfaces
{
    /// <summary>
    /// Interface for display operations, abstracting the UI layer
    /// </summary>
    public interface IDisplayAdapter
    {
        /// <summary>
        /// Display a message on the calculator screen
        /// </summary>
        /// <param name="message">The message to display</param>
        /// <param name="scroll">Whether to enable scrolling</param>
        void Display(string message, bool scroll);

        /// <summary>
        /// Print a line to the printer
        /// </summary>
        /// <param name="line">The line to print</param>
        /// <param name="justify">Justification character (L/R)</param>
        void Print(string line, char justify);

        /// <summary>
        /// Send a line to the printer (alternative method name used in some contexts)
        /// </summary>
        void ToPrinter(string line, char justify);
    }
}
