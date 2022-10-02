using AutoMapper;
using CardStorageServices.Data;
using CardStorageServices.Models;
using CardStorageServices.Models.Request.CardRequestResponse;

namespace CardStorageServices.Mapping
{
    public class MappingsProfile:Profile
    {
        public MappingsProfile()
        {
            CreateMap<Card, CardDto>();//Card-тип от которого будет идти, CardDto- тип  к которому будем приводить
            CreateMap<CreateCardRequest, Card>();
        }
    }
}
