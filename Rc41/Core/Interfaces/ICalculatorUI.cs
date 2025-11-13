namespace Rc41.Core.Interfaces
{
    /// <summary>
    /// Combined interface for all UI adapter operations.
    /// This provides a single point of contact for the core logic to interact with the UI.
    /// </summary>
    public interface ICalculatorUI : IDisplayAdapter, IAnnunciatorAdapter, ITimerAdapter, IPrinterAdapter, IDebugAdapter, ICardReaderAdapter
    {
    }
}
