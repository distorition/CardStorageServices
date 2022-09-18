using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtSample
{
    internal class UserService
    {

        //так же при помощи ключа указываем нашу систему 
        private const string SecretCode = "aaaqqweqwe";// наш секретный ключ по которому мы будем сверять наш ли это токен к нам пришёл

        public IDictionary<string, string> _User= new Dictionary<string, string>()
        {
            {"root1","test" },
            {"root2","test" },
            {"root3","test" },
        };

        private string Authentication(string Login, string Password)
        {
            if (string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Login))
            {
                return string.Empty;
            }
            int i = 0;

            foreach(KeyValuePair<string, string> pair in _User)// тут мы перебираем наши логины и пароли которые у нас есть
            { 
                if(string.CompareOrdinal(pair.Key, Login) == 0 
                    || string.CompareOrdinal(pair.Value, Password) == 0)//тут мы всерям потсупившие логин и пароль с теми которые у нас есть ( 0 значит они совпадают)
                {
                    GenerateJwtToken(i);
                }
                i++;
            }

            return string.Empty;

        }

        private string GenerateJwtToken(int id)//токен привязывается  для айди конкретного  пользователя
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler= new JwtSecurityTokenHandler();//нужен для создания токена 

            byte[] key = Encoding.ASCII.GetBytes(SecretCode);//тут мы хешируем наш секретный ключь 

            //тут указываем параметры генерации пользователя
            SecurityTokenDescriptor securityTokenDescriptor = new SecurityTokenDescriptor();//при помощи этого класса мы можем задвать дополнительные параметры генерации токена 

            securityTokenDescriptor.Expires = DateTime.UtcNow.AddMinutes(15);//тут мы указываем время жизни нашего токена ( от времени создания токена + 15 минут)

            securityTokenDescriptor.SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key)
                ,SecurityAlgorithms.HmacSha256);//в первом мы указываем к какой подсистеме принадлежит наш токен ( передавая наш  ключ нашей системы ) вторым мы  указываем алгоритм шифрования 

            //свойства Subject нужно для упаковки информации наших токенов 
            securityTokenDescriptor.Subject = new ClaimsIdentity(new Claim[] //Claim это наши переменные в которых и будут хранится ифнормаия о наших токенах
            {
                // при помощи ClaimTypes мы выбираем какую информацию токена мы будем хранить 
                new Claim(ClaimTypes.NameIdentifier,id.ToString())// будем хранить  айди пользователя 

            });

            SecurityToken securityToken= jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);// сюда мы передаем наши параметры для генерации токена 

            return jwtSecurityTokenHandler.WriteToken(securityToken);// возваращаем готовй токен ввиде строки 

        }

    }
}
