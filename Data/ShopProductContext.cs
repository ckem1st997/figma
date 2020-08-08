
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using figma.Models;
using Microsoft.EntityFrameworkCore;

namespace figma.Data
{

    //dotnet ef migrations add InitialCreate1
    //dotnet ef database update
    public class ShopProductContext : DbContext
    {
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


        public virtual DbSet<Orders> Orders { get; set; }

        public virtual DbSet<OrderDetails> OrderDetails { get; set; }

        public virtual DbSet<ProductSizeColor> ProductSizeColors { get; set; }
        public virtual DbSet<Articles> Articles { get; set; }
        public virtual DbSet<ArticleCategories> ArticleCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TagProducts>().HasKey(t => new { t.ProductID, t.TagID });
            modelBuilder.Entity<TagProducts>().HasOne(pt => pt.Tags).WithMany(p => p.TagProducts).HasForeignKey(pt => pt.TagID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TagProducts>().HasOne(pt => pt.Products).WithMany(p => p.TagProducts).HasForeignKey(pt => pt.ProductID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Products>().HasKey(h => h.ProductID);
            modelBuilder.Entity<Orders>().HasKey(h => h.OrdersID);

            modelBuilder.Entity<OrderDetails>().HasKey(t => new { t.OrdersID, t.ProductID });
            modelBuilder.Entity<OrderDetails>().HasOne(pt => pt.Orders).WithMany(p => p.OrderDetails).HasForeignKey(pt => pt.OrdersID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderDetails>().HasOne(pt => pt.Products).WithMany(p => p.OrderDetails).HasForeignKey(pt => pt.ProductID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductSizeColor>().HasOne(p => p.Color).WithMany(b => b.ProductSizeColors).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductSizeColor>().HasOne(p => p.Size).WithMany(b => b.ProductSizeColors).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<ProductSizeColor>().HasOne(p => p.Products).WithMany(b => b.).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Articles>().HasOne(p => p.ArticleCategories).WithMany(b => b.Articles).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Carts>().HasKey(h => h.RecordID);
            modelBuilder.Entity<Carts>().HasOne(p => p.Products).WithMany(b => b.Carts).HasForeignKey(p => p.ProductID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Collection>().HasKey(h => h.CollectionID);
            modelBuilder.Entity<Products>().HasOne(p => p.Collection).WithMany(b => b.Products).HasForeignKey(p => p.CollectionID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<ProductLike>().HasOne(p => p.Products).WithMany(b => b.ProductLikes).HasForeignKey(p => p.ProductID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ReviewProduct>().HasOne(p => p.Products).WithMany(b => b.ReviewProducts).HasForeignKey(p => p.ProductId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductCategories>().HasKey(p => p.ProductCategorieID);


            modelBuilder.Entity<Products>().HasOne(p => p.ProductCategories).WithMany(b => b.Products).HasForeignKey(p => p.ProductCategorieID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<ProductLike>().HasOne(p => p.Members).WithMany(b => b.ProductLikes).HasForeignKey(p => p.MemberId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Products>().Property(p => p.CreateDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Products>().Property(p => p.CreateBy).HasDefaultValue("admin");
            modelBuilder.Entity<Products>().Property(p => p.Sort).HasDefaultValue(1);





            //modelBuilder.Entity<Carts>().HasOne(p => p.Products).WithMany(b => b.Carts).HasForeignKey(p=>p.ProductID).IsRequired(true).OnDelete(DeleteBehavior.Cascade);



            //     modelBuilder.Entity<ProductSize>().ToTable("ProductSize").Property(a => a.SizeID);
            //     modelBuilder.Entity<Test>().Property(t => t.ngayTao).HasDefaultValueSql("getdate()");
            //     modelBuilder.Entity<Test>().Property(t => t.ngayUpdate).HasDefaultValueSql("getdate()");
            //     modelBuilder.Entity<Product>().Property(t => t.CreatedDate).HasDefaultValueSql("getdate()");
            //     modelBuilder.Entity<Product>().Property(t => t.ModifiedDate).HasDefaultValueSql("getdate()");
            //     modelBuilder.Entity<Product>().Property(t => t.Status).HasDefaultValue(1);

            //     modelBuilder.Entity<ProductCategory>().Property(t => t.CreatedDate).HasDefaultValueSql("getdate()");
            //     modelBuilder.Entity<ProductCategory>().Property(t => t.Status).HasDefaultValue(1);

            //     modelBuilder.Entity<ProductCategory>().Property(t => t.Status).HasDefaultValue(1);
            //     modelBuilder.Entity<ProductCategory>().Property(t => t.ShowOnHome).HasDefaultValue(1);
            ////     modelBuilder.Entity<ProductCategory>().Property(t => t.ModifiedDate).HasDefaultValueSql("getdate()");
            //     modelBuilder.Entity<About>().ToTable("About")
            //         .Property(e => e.MetaTitle)
            //         .IsUnicode(true);

            //     modelBuilder.Entity<About>().ToTable("About")
            //         .Property(e => e.CreatedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<About>().ToTable("About")
            //         .Property(e => e.ModifiedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<About>().ToTable("About")
            //         .Property(e => e.MetaDescriptions)
            //         .IsFixedLength();

            //     modelBuilder.Entity<Category>().ToTable("Category")
            //         .Property(e => e.MetaTitle)
            //         .IsUnicode(true);

            //     modelBuilder.Entity<Category>().ToTable("Category")
            //         .Property(e => e.CreatedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<Category>().ToTable("Category")
            //         .Property(e => e.ModifiedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<Category>().ToTable("Category")
            //         .Property(e => e.MetaDescriptions)
            //         .IsFixedLength();

            //     modelBuilder.Entity<Content>().ToTable("Content")
            //         .Property(e => e.MetaTitle)
            //         .IsUnicode(true);

            //     modelBuilder.Entity<Content>().ToTable("Content")
            //         .Property(e => e.CreatedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<Content>().ToTable("Content")
            //         .Property(e => e.ModifiedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<Content>().ToTable("ContentContentTag")
            //         .Property(e => e.MetaDescriptions)
            //         .IsFixedLength();

            //     modelBuilder.Entity<ContentTag>().ToTable("ContentTag")
            //         .Property(e => e.TagID).IsUnicode(false);
            //     modelBuilder.Entity<ContentTag>().HasKey(t => new { t.ContentID, t.TagID });

            //     modelBuilder.Entity<Footer>().ToTable("Footer")
            //         .Property(e => e.FooterID)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<Order>().ToTable("Order")
            //         .Property(e => e.ShipMobile)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<OrderDetail>().ToTable("OrderDetail")
            //         .Property(e => e.Price);
            //     modelBuilder.Entity<OrderDetail>().ToTable("OrderDetail")
            //   .HasKey(t => new { t.ProductID, t.OrderID });

            //     modelBuilder.Entity<Product>().ToTable("Product")
            //         .Property(e => e.ProductCode)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<Product>().ToTable("Product")
            //         .Property(e => e.MetaTitle)
            //         .IsUnicode(true);

            //     modelBuilder.Entity<Product>().ToTable("Product")
            //         .Property(e => e.Price);

            //     modelBuilder.Entity<Product>().ToTable("Product")
            //         .Property(e => e.PromotionPrice);

            //     modelBuilder.Entity<Product>().ToTable("Product")
            //         .Property(e => e.CreatedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<Product>().ToTable("Product")
            //         .Property(e => e.ModifiedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<Product>().ToTable("Product")
            //         .Property(e => e.MetaDescriptions)
            //         .IsFixedLength();

            //     modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory")
            //         .Property(e => e.MetaTitle)
            //         .IsUnicode(true);

            //     modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory")
            //         .Property(e => e.CreatedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory")
            //         .Property(e => e.ModifiedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<ProductCategory>().ToTable("ProductCategory")
            //         .Property(e => e.MetaDescriptions)
            //         .IsFixedLength();

            //     modelBuilder.Entity<Slide>().ToTable("Slide")
            //         .Property(e => e.CreatedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<Slide>().ToTable("Slide")
            //         .Property(e => e.ModifiedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<SystemConfig>().ToTable("SystemConfig")
            //         .Property(e => e.ConfigID)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<SystemConfig>().ToTable("SystemConfig")
            //         .Property(e => e.Type)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<SystemConfig>().ToTable("SystemConfig")
            //         .HasNoKey();

            //     modelBuilder.Entity<Tag>().ToTable("Tag")
            //         .Property(e => e.TagID)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<User>().ToTable("User")
            //         .Property(e => e.UserName)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<User>().ToTable("User")
            //         .Property(e => e.Password)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<User>().ToTable("User")
            //         .Property(e => e.CreatedBy)
            //         .IsUnicode(false);

            //     modelBuilder.Entity<User>().ToTable("User")
            //         .Property(e => e.ModifiedBy)
            //         .IsUnicode(false);
        }


        //public DbSet<> Students { get; set; }
        //public DbSet<Enrollment> Enrollments { get; set; }
        //public DbSet<Course> Courses { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Course>().ToTable("Course");
        //    modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
        //    modelBuilder.Entity<Student>().ToTable("Student");
        //}


    }
}
