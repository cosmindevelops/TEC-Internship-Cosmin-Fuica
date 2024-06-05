namespace ApiApp.Common.Exceptions;

public class SalaryNotFoundException : Exception
{
    public SalaryNotFoundException(string message) : base(message)
    {
    }
}