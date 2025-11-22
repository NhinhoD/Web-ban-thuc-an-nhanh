using ASM_C_4.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ASM_C_4.Repository
{
	public class DataContext : IdentityDbContext<AppUserModel>
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		public DbSet<BrandModel> Brands { get; set; }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<OrderModel> Orders { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboProduct> ComboProducts { get; set; }
        public DbSet<RatingModel> Ratings { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // Bắt buộc EF Core sử dụng tên bảng có dấu ngoặc kép và không thay đổi
            // Nếu bạn đang dùng schema 'public' (mặc định của Supabase)
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                // Kiểm tra xem entity có phải là bảng Identity không
                if (entity.GetTableName().StartsWith("AspNet"))
                {
                    // Đặt tên bảng với dấu ngoặc kép để PostgreSQL phân biệt hoa/thường
                    entity.SetTableName(entity.GetTableName());

                    // Đảm bảo tên khóa được viết hoa chính xác (tùy chọn)
                    foreach (var key in entity.GetKeys())
                    {
                        key.SetName(key.GetName());
                    }
                }
            }
            modelBuilder.Entity<ComboProduct>()
                .HasKey(cp => new { cp.ComboId, cp.ProductId });

            modelBuilder.Entity<ComboProduct>()
                .HasOne(cp => cp.Combo)
                .WithMany(c => c.ComboProducts)
                .HasForeignKey(cp => cp.ComboId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ComboProduct>()
                .HasOne(cp => cp.Product)
                .WithMany()
                .HasForeignKey(cp => cp.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }



    }
}
