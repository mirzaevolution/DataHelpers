namespace DataHelpers
{
    /// <summary>
    /// A generic class that holds a value of certain operation and status result class
    /// which can be used to detect whether certain operation succeeded or not
    /// </summary>
    /// <typeparam name="T">Any type</typeparam>
    public class DataResult<T>
    {
        public DataResult(T data, StatusResult status)
        {
            Data = data;
            Status = status;
        }
        /// <summary>
        /// Main value passed as a result
        /// </summary>
        public T Data { get; private set; }
        /// <summary>
        /// Status of the operation
        /// </summary>
        public StatusResult Status { get; private set; }
    }
}
