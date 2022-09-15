using CardStorageServices.Models.Interfaces;

namespace CardStorageServices.Models.Request.CardRequestResponse
{
    public class CreateCardResponse : IOperationResult
    {
        public string? CardId { get; set; }
        public int ErrorCode { get; set; }

        public string? ErorrMessage { get; set; }
    }
}
