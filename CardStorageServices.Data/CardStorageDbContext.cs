using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardStorageServices.Data
{
    public class CardStorageDbContext : DbContext
    {
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public CardStorageDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
