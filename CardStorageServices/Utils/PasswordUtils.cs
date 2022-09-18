using System.Security.Cryptography;
using System.Text;

namespace CardStorageServices.Utils
{

    /// <summary>
    /// тут мы сохраняем и храним пароли 
    /// </summary>
    public static class PasswordUtils
    {
        private const string SecretKey = "1234asrf324";


        /// <summary>
        /// Соль это то что мы используем для хранения паролей в бд чтобы сделать пароль более защищенным 
        /// Секретный ключ также нужен для этого ( в конечном итоге наш пароль будет состоять из ( пароль+соль(которую рандомно генерируем)+ наш секретный ключ)
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static (string passwordSlatm, string passwordHash) CreatePasswordHash(string password)//на выход мы получаем Соль и Хеш от пароля 
        {
            //генерируем рандомную соль
            byte[] buffer = new byte[16];
            RNGCryptoServiceProvider securityRandom= new RNGCryptoServiceProvider();//класс нужен для запонения массива рандомной последовательностью байт 
            securityRandom.GetBytes(buffer);

            //создаем хеш
            string passwordSlatm=Convert.ToBase64String(buffer);//это нужно для того чтобы послдовательность байт представить ввиде строки 
            string passwordHash = GetPasswordHash(password, passwordSlatm);

            return (passwordHash, passwordSlatm);
        }

        /// <summary>
        /// когда пользователь пытается авторизоваться мы бдуем брать у некго пароль и соль а потом бдуем сравнивать совпадает ли его хеш 
        /// который мы получим при генерации с тем хешем который есть у нас в базе данных
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordSlatm"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
        public static bool VerifyPassword(string password, string passwordSlatm, string passwordHash)
        {
            return GetPasswordHash(password, passwordSlatm) == passwordHash; 
        }


        /// <summary>
        /// метод нужен для подготовки конечного вида пароля , создаем наш хешь
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        public static string GetPasswordHash(string password,string passwordSalt)
        {
            //создаем пароль в строку 
            password = $"{password}~{passwordSalt}~{SecretKey}";// то из чего будет состоять наш пароль ( пароль+соль+секретный ключ)

            byte[] buffer = Encoding.UTF8.GetBytes(password);// тут мы снова наш конечный пароль перегоняем в массив байт 

            SHA512 sHA512 = new SHA512Managed();
            byte[] passwordHash = sHA512.ComputeHash(buffer);// а тут мы уже вычисляем хешь нашего конечного пароля 

            return Convert.ToBase64String(passwordHash);
        }
    }
}
