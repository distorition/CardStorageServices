using Grpc.Net.Client;
using static CardStorageServicesProtos.CardService;
using static CardStorageServicesProtos.ClientService;

class Programm
{


    static void Main(string[] args)
    {


        /// <summary>
        /// таким образом мы говори что поддерживаем работу с незащищённым соединением 
        /// </summary>
        AppContext.SetSwitch("System.Net.Hpp.SocketHttpHandler Http2UnencryptedSupport", true);//если мы используем протоколы Http а не Https

        //CardServiceClient

        //ClientServiceClient


        using var chanel = GrpcChannel.ForAddress("http://localhost:5001");//создаем наш канал 
        ClientServiceClient ClientServiceClient = new ClientServiceClient(chanel);
        var response = ClientServiceClient.Create(new CardStorageServicesProtos.CreateClientRequest//создаем клиента
        {
            FirstName = "aaa",
            Patronomic = "ssss",
            SurName = "asdasd"
        });

        Console.WriteLine($"Client {response.ClietnId} create sucefules");

        CardServiceClient cardServic = new CardServiceClient(chanel);
        var getByClientIdResponse = cardServic.getBuClientId(new CardStorageServicesProtos.GetBuClientIdRequest
        {
            ClietnId = 1,

        });



        foreach (var card in getByClientIdResponse.Cards)
        {
            Console.WriteLine($"{card.CardNO}; {card.Name}; {card.CVV2}; {card.ExpDate}");
        }
    }
}