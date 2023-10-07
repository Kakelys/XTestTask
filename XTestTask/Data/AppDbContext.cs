using Microsoft.EntityFrameworkCore;
using XTestTask.Data.Models;

namespace XTestTask.Data
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<ChatMember> ChatMembers { get; set; } = null!;
        public virtual DbSet<ChatMessage> ChatMessages { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Account>(a => 
            {
                a.HasKey(a => a.Id);

                a.HasIndex(a => a.Name)
                    .IsUnique();

                a.Property(a => a.Id)
                    .ValueGeneratedOnAdd();

                a.Property(a => a.Name)
                    .IsRequired();

                a.HasMany(a => a.CreatedChats)
                    .WithOne(c => c.Creator)
                    .HasForeignKey(c => c.CreatorId)
                    .OnDelete(DeleteBehavior.Cascade);

                a.HasMany(a => a.Chats)
                    .WithOne(c => c.Account)
                    .HasForeignKey(c => c.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Chat>(c => 
            {
                c.HasKey(c => c.Id);

                c.HasIndex(c => c.Name)
                    .IsUnique();

                c.Property(c => c.Id)
                    .ValueGeneratedOnAdd();

                c.Property(c => c.Name)
                    .IsRequired();

                c.HasOne(c => c.Creator)
                    .WithMany(a => a.CreatedChats)
                    .HasForeignKey(c => c.CreatorId)
                    .OnDelete(DeleteBehavior.Cascade);

                c.HasMany(c => c.Members)
                    .WithOne(cm => cm.Chat)
                    .HasForeignKey(cm => cm.ChatId)
                    .OnDelete(DeleteBehavior.Cascade);

                c.HasMany(c => c.Messages)
                    .WithOne(cm => cm.Chat)
                    .HasForeignKey(cm => cm.ChatId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ChatMember>(cm => 
            {
                cm.HasKey(cm => new { cm.ChatId, cm.AccountId });

                cm.HasOne(cm => cm.Chat)
                    .WithMany(c => c.Members)
                    .HasForeignKey(cm => cm.ChatId)
                    .OnDelete(DeleteBehavior.Cascade);

                cm.HasOne(cm => cm.Account)
                    .WithMany(a => a.Chats)
                    .HasForeignKey(cm => cm.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                cm.HasMany(cm => cm.Messages)
                    .WithOne(m => m.Member)
                    .HasForeignKey(m => m.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ChatMessage>(cm => 
            {
                cm.HasKey(cm => cm.Id);

                cm.Property(cm => cm.Id)
                    .ValueGeneratedOnAdd();

                cm.Property(cm => cm.Message)
                    .IsRequired();

                cm.Property(cm => cm.CreatedAt)
                    .IsRequired();

                cm.HasOne(cm => cm.Chat)
                    .WithMany(c => c.Messages)
                    .HasForeignKey(cm => cm.ChatId)
                    .OnDelete(DeleteBehavior.Cascade);

                cm.HasOne(cm => cm.Member)
                    .WithMany(m => m.Messages)
                    .HasForeignKey(cm => new {cm.ChatId, cm.MemberId})
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(builder);
        }
    }
}