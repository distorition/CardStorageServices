namespace CardStorageServices.Models.Request.AuthenticationRequestResponse
{
    /// <summary>
    /// класс для логина и пароля  нашего пользователя 
    /// </summary>
    public class AuthenticationRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
