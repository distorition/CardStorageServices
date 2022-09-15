using CardStorageServices.Data;

namespace CardStorageServices.Services.Impl
{
    public class ClientRepository:IClientPerositoryServices
    {
        private readonly ILogger<CardRepository> _Logger;
        private readonly CardStorageDbContext _Context;

        public ClientRepository(ILogger<CardRepository> logger, CardStorageDbContext context)
        {
            _Logger = logger;
            _Context = context;
        }

        public int Create(Client data)
        {
            _Context.Clients.Add(data);
            _Context.SaveChanges();
            return data.ClientId;
        }

        public int Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IList<Client> GetAll()
        {
            throw new NotImplementedException();
        }

        public Client GetById(string id)
        {
            throw new NotImplementedException();
        }

        public int Update(Client data)
        {
            throw new NotImplementedException();
        }

        string IRepository<Client, string>.Create(Client data)
        {
            throw new NotImplementedException();
        }
    }
}
