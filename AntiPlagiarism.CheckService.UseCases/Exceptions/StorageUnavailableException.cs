namespace AntiPlagiarism.CheckService.UseCases.Exceptions;

public class StorageUnavailableException : Exception
{
    public StorageUnavailableException()
    {
    }

    public StorageUnavailableException(string message)
        : base(message)
    {
    }

    public StorageUnavailableException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}