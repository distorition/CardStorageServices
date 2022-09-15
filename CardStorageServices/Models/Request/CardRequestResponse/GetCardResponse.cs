using CardStorageServices.Models.Interfaces;

namespace CardStorageServices.Models.Request.CardRequestResponse
{
    public class GetCardResponse : IOperationResult
    {
        public IList<CardDto> Cards { get; set; }
        public int ErrorCode { get; set; }

        public string? ErorrMessage { get; set; }
    }
}
