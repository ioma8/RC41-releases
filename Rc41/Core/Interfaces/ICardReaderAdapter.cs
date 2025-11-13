namespace Rc41.Core.Interfaces
{
    /// <summary>
    /// Interface for card reader dialog operations
    /// </summary>
    public interface ICardReaderAdapter
    {
        /// <summary>
        /// Show dialog to load a card and return the filename
        /// </summary>
        /// <returns>Filename selected or null if cancelled</returns>
        string? LoadCard();

        /// <summary>
        /// Show dialog to save a card and return the filename
        /// </summary>
        /// <returns>Filename selected or null if cancelled</returns>
        string? SaveCard();
    }
}
