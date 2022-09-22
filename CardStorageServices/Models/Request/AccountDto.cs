namespace CardStorageServices.Models.Request
{
    /// <summary>
    /// класс который нужен для возвращении информации об пользователей 
    /// что будет отображать на странице после авторизации 
    /// </summary>
    public class AccountDto
    {
        public int AccountId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondaryName { get; set; }
        public string Email { get; set; }
        public bool Locked { get; set; }
    }
}
