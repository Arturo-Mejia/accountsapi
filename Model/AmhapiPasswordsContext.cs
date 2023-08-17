using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace apitest.Model;

public partial class AmhapiPasswordsContext : DbContext
{
    public AmhapiPasswordsContext()
    {
    }

    public AmhapiPasswordsContext(DbContextOptions<AmhapiPasswordsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=sql.bsite.net\\MSSQL2016;Database=amhapi_Passwords;User Id=amhapi_Passwords;Password=Amh/151298;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("accounts");

            entity.Property(e => e.Account1).HasColumnName("account");
            entity.Property(e => e.Iduser).HasColumnName("iduser");
            entity.Property(e => e.Pass).HasColumnName("pass");

            entity.HasOne(d => d.IduserNavigation).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.Iduser)
                .HasConstraintName("FK_accounts_accounts");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Email).HasColumnName("email");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
