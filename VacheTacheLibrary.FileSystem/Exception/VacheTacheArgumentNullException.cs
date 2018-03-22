namespace VacheTacheLibrary.FileSystem.Exception
{
    /// <summary>This exception is the VacheTache version of a <see cref="System.ArgumentNullException"/>.
    /// </summary>
    [System.Serializable]
    public class VacheTacheArgumentNullException : System.ArgumentNullException
    {
        public VacheTacheArgumentNullException(string paramName, string message)
            :base(paramName, message)
        {
        }

        public VacheTacheArgumentNullException() { }
        public VacheTacheArgumentNullException(string paramName) : base(paramName) { }
        public VacheTacheArgumentNullException(string message, System.Exception inner) : base(message, inner) { }
        protected VacheTacheArgumentNullException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
