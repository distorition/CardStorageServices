using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageServices.Data
{
    [Table("Clients")]
    public class Client
    {

        /// <summary>
        /// key- это значит что он будет являтся первичным ключём 
        /// DatabaseGeneratedOption- делает так чтобы каждому новому обьекту сама бд давала индентификатор
        /// </summary>
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClientId { get; set; }

        [Column]
        [StringLength(255)]
        public string? SurName { get; set; }
        [Column]
        [StringLength(255)]
        public string? FirstName { get; set; }
        [Column]
        [StringLength(255)]
        public string? Patronomic { get; set; }

        [InverseProperty(nameof(Card.Client))]
        public virtual ICollection<Card> Cards { get; set; }= new HashSet<Card>();//так мы получаем список карт которые есть у клиента 
    }
}
