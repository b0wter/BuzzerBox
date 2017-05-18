using BuzzerEntities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BuzzerBox.Data
{
    public interface IDatabaseContextProvider
    {
        DbSet<Room> Rooms { get; set; }
        DbSet<Question> Questions { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<Response> Responses { get; set; }
        DbSet<Vote> Votes { get; set; }
        DbSet<RegistrationToken> RegistrationTokens { get; set; }
        DbSet<SessionToken> SessionTokens { get; set; }
        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }
        IModel Model { get; }
        int SaveChanges();
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken);
        void Dispose();
        EntityEntry Entry(object entity);
        EntityEntry Add(object entity);
        Task<EntityEntry> AddAsync(object entity, CancellationToken cancellationToken);
        EntityEntry Attach(object entity);
        EntityEntry Update(object entity);
        EntityEntry Remove(object entity);
        void AddRange(params object[] entities);
        Task AddRangeAsync(params object[] entities);
        void AttachRange(params object[] entities);
        void UpdateRange(params object[] entities);
        void RemoveRange(params object[] entities);
        void AddRange(IEnumerable<object> entities);
        Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken);
        void AttachRange(IEnumerable<object> entities);
        void UpdateRange(IEnumerable<object> entities);
        void RemoveRange(IEnumerable<object> entities);
        object Find(Type entityType, params object[] keyValues);
        Task<object> FindAsync(Type entityType, params object[] keyValues);
        Task<object> FindAsync(Type entityType, object[] keyValues, CancellationToken cancellationToken);
    }

    public class BuzzerContext : DbContext, IDatabaseContextProvider
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<RegistrationToken> RegistrationTokens {get;set;}
        public DbSet<SessionToken> SessionTokens { get; set; }

        public BuzzerContext(DbContextOptions<BuzzerContext> options) : base(options)
        {
            //
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // the room entity is on one end of the depedency chain
            modelBuilder.Entity<Room>().ToTable("Rooms");
            // the user entity is on one end of the dependency chain
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<User>().Property(u => u.Name).IsRequired();

            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<Question>().HasOne(q => q.Room).WithMany(r => r.Questions).HasForeignKey(q => q.RoomId);
            modelBuilder.Entity<Question>().HasOne(q => q.User).WithMany(u => u.Questions).HasForeignKey(q => q.UserId);

            modelBuilder.Entity<Response>().ToTable("Responses");
            modelBuilder.Entity<Response>().HasOne(r => r.Question).WithMany(q => q.Responses).HasForeignKey(r => r.QuestionId);

            modelBuilder.Entity<Vote>().ToTable("Votes");
            modelBuilder.Entity<Vote>().HasOne(v => v.Response).WithMany(r => r.Votes).HasForeignKey(v => v.ResponseId);
            modelBuilder.Entity<Vote>().HasOne(v => v.User).WithMany(u => u.Votes).HasForeignKey(v => v.UserId);

            // RegistrationTokens dont need a reference to the user whom they are used by.
            modelBuilder.Entity<RegistrationToken>().ToTable("RegistrationTokens");
            modelBuilder.Entity<RegistrationToken>().HasAlternateKey(s => s.Token).HasName("AlternateKey_Token");

            modelBuilder.Entity<SessionToken>().ToTable("SessionTokens");
            modelBuilder.Entity<SessionToken>().HasOne(t => t.User).WithMany(u => u.SessionToken).HasForeignKey(t => t.UserId);
            modelBuilder.Entity<SessionToken>().HasAlternateKey(s => s.Token).HasName("AlternateKey_Token");
        }
    }
}
