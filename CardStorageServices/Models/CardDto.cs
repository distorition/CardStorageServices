namespace CardStorageServices.Models
{
    public class CardDto
    {
        public string CardNO { get; set; }
        public string? Name { get; set; }
        public string? CVV2 { get; set; }
        public string ExpDate { get; set; }
    }
}
