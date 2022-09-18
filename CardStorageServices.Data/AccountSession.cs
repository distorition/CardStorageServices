using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageServices.Data
{
    [Table("AccountSession")]
    public class AccountSession
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SessionId { get; set; }//  у каждой сесси будет свой ид 

        [Required]
        [StringLength(244)]
        public string SessionToken { get; set; }// тут будет ид нашего токена 

        [ForeignKey(nameof(Account))]
        public int AccountID { get; set; }//будет хранить ссылку на пользователя ( чтобы знать что токен именно нашего пользователя)

        [Column(TypeName ="dateTime2")]
        public DateTime CreatedDate { get; set; }

        [Column(TypeName = "dateTime2")]
        public DateTime TimeLastRequest { get; set; }//время активности нашей сесси (когда пользователь заходит на наш сайт)

        public bool IsClosed { get;set; }

        [Column(TypeName = "dateTime2")]
        public DateTime? TimeClosed { get; set; }

        public virtual Account Account { get; set; }
    }
}
