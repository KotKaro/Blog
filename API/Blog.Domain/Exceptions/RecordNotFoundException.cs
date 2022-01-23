using System;

namespace Blog.Domain.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        private const string ExceptionMessage = "Record of type: {0} - with Id: {1} - hasn't been found";

        public RecordNotFoundException(Type recordType, object recordId)
            : base(string.Format(ExceptionMessage,nameof(recordType), recordId))
        {
        }
    }

    public class InvalidValueException : ArgumentException
    {
        public InvalidValueException(string message, string valueName) : base(message, valueName)
        {
        }
    }
}
