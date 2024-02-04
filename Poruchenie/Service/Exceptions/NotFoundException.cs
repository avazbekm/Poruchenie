namespace Poruchenie.Service.Exceptions;

public class NotFoundException(string message) : Exception(message)
{
    public int MyProperty { get; set; } = 404;
}
