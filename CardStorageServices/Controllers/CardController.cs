using AutoMapper;
using CardStorageServices.Data;
using CardStorageServices.Models;
using CardStorageServices.Models.Request.CardRequestResponse;
using CardStorageServices.Services.Impl;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CardStorageServices.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _Logger;
        private readonly ICardRepositoryServices _CardRepositoryServices;
        private readonly IMapper mapper;
        private readonly IValidator<CreateCardRequest> validator;

        public CardController(ILogger<CardController> logger, ICardRepositoryServices cardRepositoryServices,IMapper mapper1, IValidator<CreateCardRequest> validator)
        {
            _Logger = logger;
            _CardRepositoryServices = cardRepositoryServices;
            mapper = mapper1;
            this.validator = validator;
        }


        [HttpGet("getAll")]
        public IActionResult GetByClientID()
        {
            _Logger.LogInformation("HetByClientID!!!");
            return Ok();
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public IActionResult Craete([FromBody] CreateCardRequest request)// метод который принимает запрос 
        {
            try
            {
                ValidationResult validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ToDictionary());//возвращаем список ошибок
                }
                //var carId = _CardRepositoryServices.Create(new Card
                //{
                //    ClientId = request.ClientId,
                //    CardNo = request.CardNo,
                //    ExData = request.ExpDate,
                //    CVV = request.CVV2
                //});
                //таким образом стракой снизу мы заменили всё что сверху
                var carId = _CardRepositoryServices.Create(mapper.Map<Card>(request));//к какому типу мы хотим привести обьект(к типу Card) что хотим привести( request)
                return Ok(new CreateCardResponse
                {
                    CardId = carId.ToString()
                });
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Create Card Error");
                return Ok(new CreateCardResponse
                {
                    ErorrMessage = ex.Message,
                    ErrorCode = 1012
                });
            }
        }


        /// <summary>
        /// метод который возвращает информацию о клиенте 
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        [HttpGet("get-by-client-id")]
        public IActionResult GetByClientId([FromQuery] string clientId)//метод который возвращает запрос 
        {
            try
            {
                var cards = _CardRepositoryServices.GetByClientId(clientId);
                return Ok(new GetCardResponse
                {
                    Cards=mapper.Map<List<CardDto>>(cards)
                    //Cards = cards.Select(card => new CardDto
                    //{
                    //    CardNO = card.CardNo,
                    //    CVV2 = card.CVV,
                    //    Name = card.Name,
                    //    ExpDate = card.ExData.ToString("MM/yy")

                    //}).ToList()
                });
            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, "Get Card error");
                return Ok(new GetCardResponse
                {
                    ErrorCode = 1010,
                    ErorrMessage = "Get Card Errors"
                });
            }
        }
    }
}
