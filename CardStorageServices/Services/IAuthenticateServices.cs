using CardStorageServices.Models.Request;
using CardStorageServices.Models.Request.AuthenticationRequestResponse;

namespace CardStorageServices.Services
{
    public interface IAuthenticateServices
    {
        /// <summary>
        /// на вход подается логи и пароль 
        /// на выход возращаем результат ( обьект который указан в самом классе SessionInfo)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        AuthenticationResponse Login(AuthenticationRequest request);

        /// <summary>
        /// нужен для проверка ( авторизирован ли пользователь)
        /// если да то он заходит на свой ак
        /// если нет то выкидываем его на страницу авторизации 
        /// </summary>
        /// <param name="sessionToken"></param>
        /// <returns></returns>
        public SessionInfo GetSessionInof(string sessionToken);
    }
}
