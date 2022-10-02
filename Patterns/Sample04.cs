using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns
{
    /// <summary>
    /// патерн строитель  отделяет  конструирвоания сложного обьекта от его представления
    /// </summary>
    internal class Sample04
    {
        static void Main(string[] args)
        {
            MailMessage mailMessage= new MailMessageBuilder()
                .To("aaaa")
                .From("ddddd")
                .Body("assdas")
                .Build();
        }
    }

    public class MailMessage
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get;set; }
        public string Body { get;set; }
    }

   public class MailMessageBuilder
    {
        private readonly MailMessage mailMessage = new MailMessage();

        public MailMessage Build()
        {
            if (string.IsNullOrEmpty(mailMessage.To))
            {
                throw new InvalidOperationException("строка кому должна быть заполнена ");
            }
            return mailMessage;
        }

        public MailMessageBuilder From(string addres)
        {
            mailMessage.From = addres;
            return this;//возвращает ссылку на текущий экземпляр класса MailMessageBuilder
        }

        public MailMessageBuilder To(string addres)
        {
            mailMessage.To = addres;
            return this;
        }

        public MailMessageBuilder Subject(string addres)
        {
            mailMessage.Subject = addres;
            return this;
        }
        public MailMessageBuilder Body(string addres)
        {
            mailMessage.Body = addres;
            return this;
        }
    }
}
