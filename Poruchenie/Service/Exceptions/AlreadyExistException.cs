namespace Poruchenie.Service.Exceptions;

public class AlreadyExistException(string message) : Exception(message)
{
    public int MyProperty { get; set; } = 403;
}