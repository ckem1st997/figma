
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using figma.Models;
using Microsoft.EntityFrameworkCore;

namespace figma.Data
{

    //dotnet ef migrations add InitialCreate1
    //dotnet ef database update
    //EntityFrameworkCore\Update-Database
    //EntityFrameworkCore\Add-Migration
    public class ShopProductContext : DbContext
    {
        public ShopProductContext()
        {
        }

        public ShopProductContext(DbContextOptions<ShopProductContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Abouts> Abouts { get; set; }

        public virtual DbSet<Admins> Admins { get; set; }
        public virtual DbSet<Members> Members { get; set; }
        public virtual DbSet<Albums> Albums { get; set; }

        public virtual DbSet<Videos> Videos { get; set; }

        public virtual DbSet<Contacts> Contacts { get; set; }

        public virtual DbSet<Carts> Carts { get; set; }
        public virtual DbSet<ProductCategories> ProductCategories { get; set; }
        public virtual DbSet<ProductLike> ProductLikes { get; set; }
        public virtual DbSet<ReviewProduct> ReviewProducts { get; set; }

        public virtual DbSet<Collection> Collections { get; set; }
        public virtual DbSet<Banners> Banners { get; set; }

        public virtual DbSet<ConfigSites> ConfigSites { get; set; }

        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<TagProducts> TagProducts { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }


        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderDetail> OrderDetails { get; set; }

        public virtual DbSet<ProductSizeColor> ProductSizeColors { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<ArticleCategory> ArticleCategories { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    // optionsBuilder.UseLazyLoadingProxies();
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TagProducts>().HasKey(t => new { t.ProductID, t.TagID });
            modelBuilder.Entity<TagProducts>().HasOne(pt => pt.Tags).WithMany(p => p.TagProducts).HasForeignKey(pt => pt.TagID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TagProducts>().HasOne(pt => pt.Products).WithMany(p => p.TagProducts).HasForeignKey(pt => pt.ProductID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Products>().HasKey(h => h.ProductID);
            modelBuilder.Entity<Order>().HasKey(h => h.Id);
            modelBuilder.Entity<OrderDetail>().HasKey(t => new { t.OrderId, t.ProductId, t.Size, t.Color });
            modelBuilder.Entity<OrderDetail>().HasOne(pt => pt.Order).WithMany(p => p.OrderDetails).HasForeignKey(pt => pt.OrderId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderDetail>().HasOne(pt => pt.Product).WithMany(p => p.OrderDetails).HasForeignKey(pt => pt.ProductId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductSizeColor>().HasOne(p => p.Color).WithMany(b => b.ProductSizeColors).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductSizeColor>().HasOne(p => p.Size).WithMany(b => b.ProductSizeColors).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Article>().HasOne(p => p.ArticleCategory).WithMany(b => b.Articles).HasForeignKey(py => py.ArticleCategoryId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Carts>().HasKey(h => h.RecordID);
            modelBuilder.Entity<Carts>().HasOne(p => p.Products).WithMany(b => b.Carts).HasForeignKey(p => p.ProductID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Collection>().HasKey(h => h.CollectionID);
            modelBuilder.Entity<Collection>().Property(p => p.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Products>().HasOne(p => p.Collection).WithMany(b => b.Products).HasForeignKey(p => p.CollectionID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ReviewProduct>().HasOne(p => p.Products).WithMany(b => b.ReviewProducts).HasForeignKey(p => p.ProductId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductCategories>().HasKey(p => p.ProductCategorieID);

            modelBuilder.Entity<Products>().HasOne(p => p.ProductCategories).WithMany(b => b.Products).HasForeignKey(p => p.ProductCategorieID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ProductLike>().HasOne(p => p.Members).WithMany(b => b.ProductLikes).HasForeignKey(p => p.MemberId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Products>().Property(p => p.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Products>().Property(p => p.CreateBy).HasDefaultValue("admin");
            modelBuilder.Entity<Products>().Property(p => p.Sort).HasDefaultValue(1);
            modelBuilder.Entity<ProductCategories>().Property(p => p.ParentId).HasDefaultValue(null);
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
