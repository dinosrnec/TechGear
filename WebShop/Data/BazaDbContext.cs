using Microsoft.EntityFrameworkCore;
using WebShop.Models;

namespace WebShop.Data
{
    public class BazaDbContext : DbContext
    {
        public BazaDbContext(DbContextOptions<BazaDbContext> options) : base(options)
        {
        }

        public DbSet<Proizvod> Proizvodi { get; set; }
        public DbSet<Korisnik> Korisnici { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Recenzija> Recenzije { get; set; }
        public DbSet<Narudzba> Narudžbe { get; set; }
        public DbSet<NarudzbaStavka> NarudzbaStavke { get; set; }
    }
}