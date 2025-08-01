namespace Framework.Interface
{
    public interface IVariableKeyInInspector<TValue>
    {
        /// <summary>
        /// The VariableKey is a unique identifier used to recognize the corresponding item specified in the Unity inspector.
        /// This value assists in locating the correct object.
        /// </summary>
        public TValue VariableKey { get; }
    }
}