using BuzzerEntities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuzzerBox.Data
{
    public class BuzzerContext : DbContext
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
            modelBuilder.Entity<Room>().ToTable("Rooms");
            modelBuilder.Entity<Question>().ToTable("Questions");
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Vote>().ToTable("Votes");
            modelBuilder.Entity<Response>().ToTable("Responses");

            modelBuilder.Entity<RegistrationToken>().ToTable("RegistrationTokens");
            modelBuilder.Entity<RegistrationToken>().HasAlternateKey(s => s.Token).HasName("AlternateKey_Token");

            modelBuilder.Entity<SessionToken>().ToTable("SessionTokens");
            modelBuilder.Entity<SessionToken>().HasAlternateKey(s => s.Token).HasName("AlternateKey_Token");
        }
    }
}
