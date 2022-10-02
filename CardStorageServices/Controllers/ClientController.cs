using AutoMapper;
using CardStorageServices.Data;
using CardStorageServices.Models.Request;
using CardStorageServices.Models.Request.CardRequestResponse;
using CardStorageServices.Services.Impl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardStorageServices.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientPerositoryServices _ClientPerositoryServices;
        private readonly IMapper mapper;
        private readonly ILogger<CardController> _logger;

        public ClientController(ILogger<CardController> logger,IClientPerositoryServices clientPerositoryServices, IMapper mapper1)
        {
            _logger = logger;
            _ClientPerositoryServices=clientPerositoryServices;
            mapper = mapper1;
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Craete([FromBody] CreateClientRequest request)// метод который принимает запрос 
        {
            try
            {
                //var clientId = _ClientPerositoryServices.Create(new Client
                //{
                //    FirstName = request.FirstName,
                //    SurName = request.SurName,
                //    Patronomic=request.Patronomic,
                //});
                var clientId= _ClientPerositoryServices.Create(mapper.Map<Client>(request));
                return Ok(new CreateClientResponse
                {
                    ClientId= clientId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Card Error");
                return Ok(new CreateCardResponse
                {
                    ErorrMessage = ex.Message,
                    ErrorCode = 1012
                });
            }
        }
    }
}
