using CardStorageServices.Data;
using CardStorageServices.Models.Request;
using CardStorageServices.Models.Request.AuthenticationRequestResponse;
using CardStorageServices.Utils;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CardStorageServices.Services.Impl
{
    public class AuthenticationServices : IAuthenticateServices
    {
        /// <summary>
        /// коллекция нужна для того чтобы брать из неё уже существующие токены и не образаться постоянно в базу данных
        /// так как это только нагружает нашу систему 
        /// </summary>
        private readonly Dictionary<string, SessionInfo> _session= new Dictionary<string, SessionInfo>();

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public const string SecretKey = "asdqweqwf231";

        public AuthenticationServices(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }


        public SessionInfo GetSessionInof(string sessionToken)
        {
            SessionInfo sessionInfo;
            lock (_session)
            {
                _session.TryGetValue(sessionToken, out sessionInfo);//пытаемся взять значения из нашей коллекции если есть то добавляем в перменную sessionInfo
            }

            if(sessionInfo == null)
            {
                using IServiceScope scope=_serviceScopeFactory.CreateScope();
                CardStorageDbContext context=scope.ServiceProvider.GetRequiredService<CardStorageDbContext>();

                AccountSession accountSession=context
                    .AccountSessions
                    .FirstOrDefault(item=>item.SessionToken==sessionToken);// если мы получим токен то при помщи токена мы сможем выйти на самого клиента 
                if(accountSession == null)
                {
                    return null;//если токена нет то выходим из метода 
                }

                Account account=context
                    .Accounts
                    .FirstOrDefault(item=>item.AccountId==accountSession.AccountID);//если сессия есть то получаем ид аккаунта который принадлежит этой сессии

                sessionInfo = GetSessionInfo(account, accountSession);//получаем информацию по сессии

                if (sessionInfo != null)
                {
                    lock (_session)
                    {
                        _session[sessionToken] = sessionInfo;// и обращаемся к нашей коллекции и привязываем таким образом токен к сессии
                    }
                }

            }

             return sessionInfo;

        }




        public AuthenticationResponse Login(AuthenticationRequest request)
        {
            using IServiceScope scrope=_serviceScopeFactory.CreateScope();//создаём область пространства scope
            
            CardStorageDbContext context=scrope.ServiceProvider.GetRequiredService<CardStorageDbContext>();// при помощи scope мы запрашиваем сервис нашей бд


            Account account = !string.IsNullOrEmpty(request.Login) //если логин не нул и не пустая строка 
                ? FindAccountByLogin(context, request.Login):null; //и метод вернул нам результат  то только тогда он помещается в переменную

            if(account == null)//если в наш accont был помещен null
            {
                return new AuthenticationResponse
                {
                    Status = AuthenticationStatus.UserNotFound
                };
            }


            if (!PasswordUtils.VerifyPassword(request.Password, account.PasswordSal, account.Passwordhash))
            {
                return new AuthenticationResponse { Status = AuthenticationStatus.InvalidPassword };
            }

            AccountSession session = new AccountSession
            {
                AccountID = account.AccountId,
                SessionToken = CreateSessionToken(account),
                CreatedDate = DateTime.Now,
                TimeLastRequest = DateTime.Now,
                IsClosed = false,

            };

            context.AccountSessions.Add(session);//добавялем информацию о сессии пользователя в бд
            context.SaveChanges();

            SessionInfo sessionInfo = GetSessionInfo(account, session);

            //блокировка пула потоков 
            lock (_session)//нужен для того чтобы сразу несколько пользователей не помешали друг дргу и не добавились в одну сессию
            {
                _session[sessionInfo.SessionToken] = sessionInfo;//добавялем внашу колекцию
            }
           

            return new AuthenticationResponse//после всего этого возвращаем успешный результат 
            {
                Status = AuthenticationStatus.Success,
                SessionInfo = sessionInfo
            };


        }


        /// <summary>
        /// метод формирующие информацию о сессии
        /// </summary>
        /// <param name="account"></param>
        /// <param name="accountSession"></param>
        /// <returns></returns>
        private SessionInfo GetSessionInfo(Account account,AccountSession accountSession)
        {

            return new SessionInfo
            {
                SessionId = accountSession.SessionId,
                SessionToken = accountSession.SessionToken,
                Account = new AccountDto
                {
                    AccountId = account.AccountId,
                    Email = account.Email,
                    FirstName = account.FirstName,
                    LastName = account.LastName,
                    SecondaryName = account.SecondName,
                    Locked = account.Locked,
                }
            };
        }

        private Account FindAccountByLogin(CardStorageDbContext context,string login)//передаем бд и логин
        {
            return context
                .Accounts
                .FirstOrDefault(account=>account.Email==login);//обращаемся к бд которую нам передали и запрашиваем оттуда емаил( обычно емаил это и есть логин) 
        }


        private string CreateSessionToken(Account account)//полный разбор метода находиться в классе UserService
        {
            JwtSecurityTokenHandler tokenHandler= new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(SecretKey);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier,account.AccountId.ToString()),
                    new Claim(ClaimTypes.Email,account.Email)
                }),
                Expires= DateTime.UtcNow.AddMinutes(15),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256),
            };

            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
