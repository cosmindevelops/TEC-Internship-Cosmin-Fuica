namespace ApiApp.Common.Exceptions;

public class DepartmentNotFoundException : Exception
{
    public DepartmentNotFoundException(string message) : base(message)
    {
    }
}