using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ServerAPICanteen.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Canteen> Canteens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure primary keys
            modelBuilder.Entity<Canteen>()
                .HasKey(c => c.IdCanteen); // Định nghĩa khóa chính
            modelBuilder.Entity<Canteen>()
                .Property(c => c.IdCanteen)
                .ValueGeneratedOnAdd(); // Tự động tăng

            modelBuilder.Entity<Category>()
                .HasKey(c => c.IdCategory); // Định nghĩa khóa chính
            modelBuilder.Entity<Category>()
                .Property(c => c.IdCategory)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<CustomerOrder>()
                .HasKey(co => co.IdOrder); // Định nghĩa khóa chính
            modelBuilder.Entity<CustomerOrder>()
                .Property(co => co.IdOrder)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Dish>()
                .HasKey(d => d.IdDish); // Định nghĩa khóa chính
            modelBuilder.Entity<Dish>()
                .Property(d => d.IdDish)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => new { od.IdOrder, od.IdDish }); // Khóa chính tổng hợp

            // Configure relationships
            modelBuilder.Entity<CustomerOrder>()
                .HasOne(co => co.User) // User liên kết với CustomerOrder
                .WithMany(u => u.CustomerOrders)
                .HasForeignKey(co => co.Id) // Id trong CustomerOrder là khóa ngoại tới User.Id
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CustomerOrder>()
                .HasOne(co => co.Canteen) // Canteen liên kết với CustomerOrder
                .WithMany(c => c.CustomerOrders)
                .HasForeignKey(co => co.IdCanteen)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dish>()
                .HasOne(d => d.Category) // Dish liên kết với Category
                .WithMany(c => c.Dishes)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.CustomerOrder) // OrderDetail liên kết với CustomerOrder
                .WithMany(co => co.OrderDetails)
                .HasForeignKey(od => od.IdOrder)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Dish) // OrderDetail liên kết với Dish
                .WithMany(d => d.OrderDetails)
                .HasForeignKey(od => od.IdDish)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure default values
            modelBuilder.Entity<CustomerOrder>()
                .Property(co => co.CreDate)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");
        }
    }
}
