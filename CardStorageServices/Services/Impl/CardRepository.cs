using CardStorageServices.Data;
using CardStorageServices.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace CardStorageServices.Services.Impl
{
    public class CardRepository:ICardRepositoryServices
    {
        private readonly ILogger<CardRepository> _Logger;
        private readonly CardStorageDbContext _Context;
        private readonly IOptions<DataBaseOptions> _databaseOptions;

        public CardRepository(ILogger<CardRepository> logger, CardStorageDbContext context,IOptions<DataBaseOptions> options)
        {
            _Logger = logger;
            _Context = context;
            _databaseOptions = options;
        }

        public string Create(Card data)
        {
            var client=_Context.Clients.FirstOrDefault(client=>client.ClientId == data.ClientId);
            if(client == null)
            {
                throw new Exception("Client not found");
            }
            _Context.Cards.Add(data);
            _Context.SaveChanges();
            return data.CardId.ToString();
        }

        public int Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Card> GetAll()
        {
            throw new NotImplementedException();
        }

        public IList<Card> GetByClientId(string id)
        {
            List<Card> cards = new List<Card>();
            using(SqlConnection sqlConnection= new SqlConnection(_databaseOptions.Value.ConnectionString))//установка соединения с базой данных
            {
                sqlConnection.Open();
                using(var sqlcomman=new SqlCommand(String.Format("Select * from cards where ClientId={0}", id), sqlConnection))//запрос к базе данных ( получеам все записи по ид клиента)
                {
                    var reader=sqlcomman.ExecuteReader();
                    while (reader.Read())
                    {
                        cards.Add(new Card
                        {
                            CardId = new Guid(reader["CardId"].ToString()),
                            CardNo = reader["CardNo"].ToString(),
                            Name = reader["Name"].ToString(),
                            CVV = reader["CVV"].ToString(),
                            ExData = Convert.ToDateTime(reader["ExData"])
                        });
                    }
                }
            }
            return cards;
        }

        public Card GetById(string id)
        {
            throw new NotImplementedException();
        }

        public int Update(Card data)
        {
            throw new NotImplementedException();
        }
    }
}
