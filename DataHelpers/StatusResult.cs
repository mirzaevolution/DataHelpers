using System.Collections.Generic;

namespace DataHelpers
{
    /// <summary>
    /// Status Result class that contains a flag and list of errors which indicate whether certain
    /// operation succeeded or not
    /// </summary>
    public class StatusResult
    {
        public StatusResult(bool isSuccess, List<string> errors)
        {
            IsSuccess = isSuccess;
            Errors = errors;
        }
        /// <summary>
        /// A flag that indicates certain operation succeeded or not
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// List of errors
        /// </summary>
        public List<string> Errors { get; set; }
    }
}
