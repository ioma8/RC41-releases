namespace Rc41.Core.Interfaces
{
    /// <summary>
    /// Interface for annunciator (indicator) operations
    /// </summary>
    public interface IAnnunciatorAdapter
    {
        /// <summary>
        /// Set the Alpha annunciator state
        /// </summary>
        void Alpha(bool state);

        /// <summary>
        /// Set the SHIFT annunciator state
        /// </summary>
        void Shift(bool state);

        /// <summary>
        /// Set the USER annunciator state
        /// </summary>
        void User(bool state);

        /// <summary>
        /// Set the PROG annunciator state
        /// </summary>
        void Prog(bool state);

        /// <summary>
        /// Set the G (Grad) annunciator state
        /// </summary>
        void G(bool state);

        /// <summary>
        /// Set the RAD (Radians) annunciator state
        /// </summary>
        void Rad(bool state);

        /// <summary>
        /// Set Flag 0 annunciator state
        /// </summary>
        void Flag_0(bool state);

        /// <summary>
        /// Set Flag 1 annunciator state
        /// </summary>
        void Flag_1(bool state);

        /// <summary>
        /// Set Flag 2 annunciator state
        /// </summary>
        void Flag_2(bool state);

        /// <summary>
        /// Set Flag 3 annunciator state
        /// </summary>
        void Flag_3(bool state);

        /// <summary>
        /// Set Flag 4 annunciator state
        /// </summary>
        void Flag_4(bool state);
    }
}
