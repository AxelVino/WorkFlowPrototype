
namespace Application.Exceptions
{
    public class ExceptionConflict : Exception
    {
        public ExceptionConflict(string message) : base(message)
        {

        }

        public ExceptionConflict(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
