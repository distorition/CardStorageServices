using CardStorageServices.Data;
using CardStorageServices.Models.Request;
using CardStorageServices.Models.Request.CardRequestResponse;
using CardStorageServices.Services.Impl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardStorageServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientPerositoryServices _ClientPerositoryServices;
        private readonly ILogger<CardController> _logger;

        public ClientController(ILogger<CardController> logger,IClientPerositoryServices clientPerositoryServices)
        {
            _logger = logger;
            _ClientPerositoryServices=clientPerositoryServices; 
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Craete([FromBody] CreateClientRequest request)// метод который принимает запрос 
        {
            try
            {
                var clientId = _ClientPerositoryServices.Create(new Client
                {
                    FirstName = request.FirstName,
                    SurName = request.SurName,
                    Patronomic=request.Patronomic,
                });
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
