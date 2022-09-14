using CardStorageServices.Data;

namespace CardStorageServices.Services.Impl
{
    public interface ICardRepositoryServices:IRepository<Card,string>
    {
        IList<Card> GetByClientId(string id);
    }
}
