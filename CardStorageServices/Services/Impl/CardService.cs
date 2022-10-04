using CardStorageServicesProtos;
using Grpc.Core;
using static CardStorageServicesProtos.CardService;

namespace CardStorageServices.Services.Impl
{
    public class CardService: CardServiceBase//этот сервис ввыполняет работу контроллера
    {
        private readonly ICardRepositoryServices cardRepositoryServices;
        
        public CardService(ICardRepositoryServices cardRepository)
        {
            cardRepositoryServices = cardRepository;
        }
        public override Task<GetBuClientIdResponse> getBuClientId(GetBuClientIdRequest request, ServerCallContext context)//это тот метод который мы указывале с реде idl(Protos)
        {
            var response=new GetBuClientIdResponse();

            response.Cards.AddRange(cardRepositoryServices.GetByClientId(request.ClietnId.ToString()).Select(card => new Card
            {
                CardNO = card.CardNo,
                CVV2 = card.CVV,
                ExpDate = card.ExData.ToShortDateString(),
                Name = card.Name,

            }).ToList());// таким образом мы из одной коллекции перекладываем данные в другую с другим типом
            
            return Task.FromResult(response);
        }
    }
}
