
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SqlOnAir.DotNet.Lib.DataClasses.BaseClasses;

namespace SqlOnAir.DotNet.Lib.DataClasses
{
    public class SoAEFContext : DbContext
    {
        public SoAEFContext(DbContextOptions<SoAEFContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            ChangeTracker.Tracked += ChangeTracker_Tracked;
        }

        private void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
        {
            if (e.Entry.Entity is SoAEntityBase entity)
            {
                entity.SetContext(this);
            }
        }


        public DbSet<LanguageToken> LanguageTokens { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<AILevel> AILevels { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<CellState> CellStates { get; set; }
        public DbSet<TicTacToeUser> TicTacToeUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasOne(e => e.Player)
                .WithMany(e => e.GamesAsPlayer)
                .HasForeignKey(e => e.TicTacToeUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Game>()
                .HasOne(e => e.Opponent)
                .WithMany(e => e.GamesAsOpponent)
                .HasForeignKey(e => e.TicTacToeUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AILevel>()
                .HasMany(e => e.Users)
                .WithOne(e => e.AILevel)
                .HasForeignKey(e => e.AILevelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cell>()
                .HasOne(e => e.Clockwise)
                .WithOne(e => e.)
                .HasForeignKey(e => e.CellId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cell>()
                .HasOne(e => e.CounterClockwise)
                .WithOne(e => e.)
                .HasForeignKey(e => e.CellId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cell>()
                .HasOne(e => e.Flip)
                .WithOne(e => e.)
                .HasForeignKey(e => e.CellId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cell>()
                .HasMany(e => e.CellStates)
                .WithMany(e => e.CurrentStateCells)
                .HasForeignKey(e => e.CellId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CellState>()
                .HasMany(e => e.CurrentStateCells)
                .WithMany(e => e.CellStates)
                .HasForeignKey(e => e.CellStateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicTacToeUser>()
                .HasOne(e => e.AILevel)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.AILevelId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicTacToeUser>()
                .HasMany(e => e.GamesAsPlayer)
                .WithOne(e => e.Player)
                .HasForeignKey(e => e.TicTacToeUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicTacToeUser>()
                .HasMany(e => e.GamesAsOpponent)
                .WithOne(e => e.Opponent)
                .HasForeignKey(e => e.TicTacToeUserId)
                .OnDelete(DeleteBehavior.Cascade);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Default configuration if needed
                optionsBuilder.UseSqlServer("Server=.,1433;Database=YourDatabase;User ID=sa;Password=YourPassword;Encrypt=false;TrustServerCertificate=true");
            }
        }
    }
}