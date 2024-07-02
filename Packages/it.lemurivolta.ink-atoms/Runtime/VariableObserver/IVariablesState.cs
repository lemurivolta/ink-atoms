namespace LemuRivolta.InkAtoms.VariableObserver
{
    /// <summary>
    /// Interface representing the access to the current ink's variables state.
    /// </summary>
    public interface IVariablesState
    {
        /// <summary>
        /// Get or set the value of a variable with given name.
        /// </summary>
        /// <param name="name">Name of the variable.</param>
        object this[string name] { get; set; }
    }
}