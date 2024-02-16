using System;
using System.Collections.Generic;
using Bootcamp.AzFunc;
using mba_es_25_grupo_02_backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace mba_es_25_grupo_02_backend
{
    public partial class dbmbaesproductionContext : DbContext,  IdentityDbContext<Pessoa>
    {
        public dbmbaesproductionContext()
        {
        }

        public dbmbaesproductionContext(DbContextOptions<dbmbaesproductionContext> options) : base(options) { }

        public virtual DbSet<Pessoa> Pessoa { get; set; }
        public virtual DbSet<Produto> Produto { get; set; }
        public virtual DbSet<Estoque> Estoque { get; set; }
        public virtual DbSet<Pedido> Pedido { get; set; }
        public virtual DbSet<PedidoProduto> PedidoProduto { get; set; }
        public virtual DbSet<ItemCarrinho> ItemCarrinho { get; set; }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:db-server-mba-es-production.database.windows.net,1433;Initial Catalog=db-mba-es-production;Persist Security Info=False;User ID=admindb;Password=FaculdadeImpact@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");
            }
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }

    internal interface IdentityDbContext<T>
    {
    }
}
