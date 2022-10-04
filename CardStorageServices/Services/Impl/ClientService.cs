using CardStorageServicesProtos;
using Grpc.Core;
using static CardStorageServicesProtos.ClientService;

namespace CardStorageServices.Services.Impl
{
    public class ClientService: ClientServiceBase//этот сервис ввыполняет работу контроллера
    {
        #region Сервисы
        private readonly IClientPerositoryServices _clientPerositoryServices;

        #endregion

        public ClientService(IClientPerositoryServices client)
        {
            _clientPerositoryServices = client;
        }
        public override Task<CreateClientResponse> Create(CreateClientRequest request, ServerCallContext context)
        {
          var clientid=_clientPerositoryServices.Create(new Data.Client
            {
                FirstName = request.FirstName,
                SurName = request.SurName,
                Patronomic=request.Patronomic,
            });

            var response = new CreateClientResponse
            {
                ClietnId = clientid
            };

            return Task.FromResult(response);
        }
    }
}
