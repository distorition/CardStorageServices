using CardStorageServices.Models.Request;
using CardStorageServices.Models.Request.AuthenticationRequestResponse;
using CardStorageServices.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;

namespace CardStorageServices.Controllers
{
    [Authorize]//теперь для работы этих методов нужно бдует передавать наш токен
    [Route("api/auth")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateServices _authenticateServices;

        public AuthenticateController(IAuthenticateServices authenticate)
        {
            _authenticateServices = authenticate;
        }

        [AllowAnonymous]//этим атрибутом мы предаставим доступ к  этому методу всем пользователям 
        [HttpPost("Login")]
        public IActionResult Login([FromBody] AuthenticationRequest authenticationRequest)
        {
            AuthenticationResponse response=_authenticateServices.Login(authenticationRequest);
            if (response.Status == AuthenticationStatus.Success)
            {
                Response.Headers.Add("X-Session_Token", response.SessionInfo.SessionToken);//передаем в заголовок страница наш токен 
            }
            return Ok(response);

        }

        [HttpGet("Session")]
        public IActionResult GetSessionInfo()
        {
            var authorization=Request.Headers[HeaderNames.Authorization];//так мы получаем корректный заголовок

           if( AuthenticationHeaderValue.TryParse(authorization, out var headerValue))//из заголовка мы бдуем получать токен сессии
            {
                var scheme=headerValue.Scheme;
                var sessionToken = headerValue.Parameter;// вот тут и полчаем параметра нашего токена сессии
                if (string.IsNullOrEmpty(sessionToken))
                {
                    return Unauthorized();
                }

                SessionInfo sessionInfo=_authenticateServices.GetSessionInof(sessionToken);//проводим проверку если нам вернули корректную сессию значит мы сможем её полчить из бд

                if (sessionInfo == null)
                {
                    return Unauthorized();
                }

                return Ok(sessionInfo);
            }
           return Unauthorized();
        }

    }
}
