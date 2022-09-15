using CardStorageServices.Models.Interfaces;

namespace CardStorageServices.Models.Request
{
    public class CreateClientResponse : IOperationResult
    {
        public string? ClientId { get; set; }
        public int ErrorCode { get; set; }

        public string? ErorrMessage { get; set; }
    }
}
