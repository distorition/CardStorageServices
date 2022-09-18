using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageServices.Data
{
    [Table("Account")]
    public class Account
    {
        /// <summary>
        /// key- это значит что данное поле будет выступать в качестве ключа 
        /// DatabaseGenerated-нужне нам для того чтобы сама база данных давала айди новым польователям 
        /// </summary>

        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [StringLength(244)]
        public string Email { get; set; }

        [StringLength(100)]
        public string PasswordSal { get; set; }

        [StringLength(244)]
        public string Passwordhash { get; set; }

        public bool Locked { get; set; }//флаг отвечающий за блокировку пользователя 

        [StringLength(244)]
        public string FirstName { get; set; }

        [StringLength(244)]
        public string LastName { get; set; }

        [StringLength(244)]
        public string SecondName { get; set; }

        [InverseProperty(nameof(AccountSession.Account))]
        public virtual ICollection<AccountSession> AccountSessions { get; set; }= new HashSet<AccountSession>();
    }
}
