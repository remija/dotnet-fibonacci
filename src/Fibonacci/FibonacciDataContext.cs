using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace Fibonacci
{
    public partial class FibonacciDataContext : DbContext
    {
        public FibonacciDataContext()
        {
        }

        public FibonacciDataContext(DbContextOptions<FibonacciDataContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TFibonacci> TFibonacci { get; set; }

      /*  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseLoggerFactory(MyLoggerFactory).UseSqlServer("Data Source=localhost,1433;Initial Catalog=FibonacciData;Integrated Security=False;User ID=sa;Password=Your_password123;MultipleActiveResultSets=True");
            }
        }*/
        
        public static readonly ILoggerFactory MyLoggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TFibonacci>(entity =>
            {
                entity.HasKey(e => e.FibId)
                    .HasName("PK_Fibonacci");

                entity.ToTable("T_Fibonacci", "sch_fib");

                entity.Property(e => e.FibId)
                    .HasColumnName("FIB_Id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.FibInput).HasColumnName("FIB_Input");

                entity.Property(e => e.FibOutput).HasColumnName("FIB_Output");
                entity.Property(e => e.FibCreatedTimestamp).HasColumnName("FIB_CreatedTimestamp");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
