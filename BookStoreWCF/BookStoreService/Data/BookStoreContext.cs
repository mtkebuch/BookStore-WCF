using System.Data.Entity;
using BookStoreService.Models;

namespace BookStoreService.Data
{
    public class BookStoreContext : DbContext
    {
        public BookStoreContext()
            : base("name=BookStoreConnection")
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().ToTable("Books");

            modelBuilder.Entity<Book>()
                .Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Book>()
                .Property(b => b.Author)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Book>()
                .Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(20);

            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasPrecision(10, 2);

            base.OnModelCreating(modelBuilder);
        }
    }
}