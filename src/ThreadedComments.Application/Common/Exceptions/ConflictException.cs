namespace ThreadedComments.Application.Common.Exceptions;


public sealed class ConflictException : AppException
{
    public ConflictException(string message) : base(message){}
}