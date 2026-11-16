using System.Data.Entity;

namespace MaSV_MyStore.Models
{
    public class MyStoreEntities : DbContext
    {
        public MyStoreEntities() : base("name=MyStoreConnection")
        {
            // Luôn xóa và tạo lại database + seed dữ liệu mẫu (dùng khi đang dev)
            Database.SetInitializer(new MyStoreInitializer());
            // Sau khi chạy ổn, đổi lại thành:
            // Database.SetInitializer(new MyStoreInitializer());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOptional(u => u.Customer)
                .WithRequired(c => c.User);

            base.OnModelCreating(modelBuilder);
        }
    }
}
