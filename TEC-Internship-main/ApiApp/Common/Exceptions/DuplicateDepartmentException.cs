namespace ApiApp.Common.Exceptions;

public class DuplicateDepartmentException : Exception
{
    public DuplicateDepartmentException(string message) : base(message)
    {
    }
}