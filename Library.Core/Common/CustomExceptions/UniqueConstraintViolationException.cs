namespace Library.Core.Common.CustomExceptions
{
    public class UniqueConstraintViolationException : Exception
    {
        public UniqueConstraintViolationException(string? message) : base(message)
        {

        }
    }
}
