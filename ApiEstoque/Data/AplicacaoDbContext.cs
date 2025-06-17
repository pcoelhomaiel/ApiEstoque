using ApiEstoque.Models;
using Microsoft.EntityFrameworkCore;
using ApiEstoque.Data.Mappings;

namespace ApiEstoque.Data
{
    public class AplicacaoDbContext : DbContext
    {
        public AplicacaoDbContext(DbContextOptions<AplicacaoDbContext> options) : base(options) {}
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ItemEstoque> ItemEstoques { get; set; }
        public DbSet<Loja> Lojas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Mapeamento();
        }

    }
}
