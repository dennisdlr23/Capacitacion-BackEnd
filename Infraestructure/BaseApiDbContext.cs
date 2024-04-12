using OrderPurchase.WebApi.Features.Users.Entities;
using OrderPurchase.WebApi.Features.Common.Entities;
using Microsoft.EntityFrameworkCore;
using OrderPurchase.WebApi.Features.Common.Dto;
using OrderPurchase.WebApi.Features.TypeDocuments.Entities;
using OrderPurchase.WebApi.Features.Orders.Entitie;

namespace OrderPurchase.WebApi.Infraestructure
{
    public class OrderPurchaseDbContext : DbContext
    {
        public OrderPurchaseDbContext(DbContextOptions<OrderPurchaseDbContext> options) : base(options)
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Theme> Theme { get; set; }
        public DbSet<TypePermission> TypePermission { get; set; }
        public DbSet<TypeDocument> TypeDocument { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new User.Map(modelBuilder.Entity<User>());
            new Permission.Map(modelBuilder.Entity<Permission>());
            new RolePermission.Map(modelBuilder.Entity<RolePermission>());
            new Role.Map(modelBuilder.Entity<Role>());
            new Theme.Map(modelBuilder.Entity<Theme>());
            new TypePermission.Map(modelBuilder.Entity<TypePermission>());

            new TypeDocument.Map(modelBuilder.Entity<TypeDocument>());
            new Order.Map(modelBuilder.Entity<Order>());
            new OrderDetail.Map(modelBuilder.Entity<OrderDetail>());
            base.OnModelCreating(modelBuilder);
        }
    }
}

