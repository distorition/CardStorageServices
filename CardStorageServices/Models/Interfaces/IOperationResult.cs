namespace CardStorageServices.Models.Interfaces
{
    public interface IOperationResult
    {
        int ErrorCode { get; }
        string? ErorrMessage { get; }
    }
}
