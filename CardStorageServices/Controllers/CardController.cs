using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardStorageServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _Logger;

        public CardController(ILogger<CardController> logger)
        {
            _Logger = logger;
        }


        [HttpGet("getAll")]
        public IActionResult GetByClientID()
        {
            _Logger.LogInformation("HetByClientID!!!");
            return Ok();
        }
    }
}
