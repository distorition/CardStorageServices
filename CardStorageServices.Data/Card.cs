using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageServices.Data
{
    [Table("Card")]
    public class Card
    {
        /// <summary>
        /// key- это значит что он будет являтся первичным ключём 
        /// DatabaseGeneratedOption- делает так чтобы каждому новому обьекту сама бд давала индентификатор
        /// </summary>
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CardId { get; set; }

        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }//это принадлежность нашей карты к какому либо из клиентов 

        [Column]
        [StringLength(20)]
        public string? Name { get; set; }

        [Column]
        [StringLength(20)]
        public string? CardNo { get; set; }

        [Column]
        [StringLength(20)]
        public string? CVV { get; set; }

        [Column]
        public DateTime ExData { get; set; }
        public virtual Client Client { get; set; }


    }
}
