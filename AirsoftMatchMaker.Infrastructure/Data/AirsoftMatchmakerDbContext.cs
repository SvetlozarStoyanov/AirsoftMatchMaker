using AirsoftMatchMaker.Infrastructure.Data.Configuration;
using AirsoftMatchMaker.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirsoftMatchMaker.Infrastructure.Data
{
    public class AirsoftMatchmakerDbContext : IdentityDbContext<User>
    {
        public AirsoftMatchmakerDbContext(DbContextOptions<AirsoftMatchmakerDbContext> options)
            : base(options)
        {
        }
        public DbSet<Weapon> Weapons { get; set; } = null!;
        public DbSet<Map> Maps { get; set; } = null!;
        public DbSet<GameMode> GameModes { get; set; } = null!;
        public DbSet<Game> Games { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<PlayerClass> PlayerClasses { get; set; } = null!;
        public DbSet<Clothing> Clothes { get; set; } = null!;
        public DbSet<Bet> Bets { get; set; } = null!;
        public DbSet<AmmoBox> AmmoBoxes { get; set; } = null!;
        public DbSet<Vendor> Vendors { get; set; } = null!;
        public DbSet<Matchmaker> Matchmakers { get; set; } = null!;
        //public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<RoleRequest> RoleRequests { get; set; } = null!;
        public DbSet<TeamRequest> TeamRequests { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Player>()
                .HasMany(u => u.Weapons)
                .WithOne(w => w.Player)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Weapon>()
                .HasOne(w => w.Player)
                .WithMany(u => u.Weapons)
                .HasForeignKey(u => u.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Game>()
                .HasOne(g => g.TeamRed)
                .WithMany(t => t.GamesAsTeamRed)
                .HasForeignKey(g => g.TeamRedId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Game>()
                .HasOne(g => g.TeamBlue)
                .WithMany(t => t.GamesAsTeamBlue)
                .HasForeignKey(g => g.TeamBlueId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Game>()
                .HasOne(g => g.Map)
                .WithMany(t => t.Games)
                .HasForeignKey(g => g.MapId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Game>()
               .HasOne(g => g.GameMode)
               .WithMany(t => t.Games)
               .HasForeignKey(g => g.GameModeId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Bet>()
                .HasOne(b => b.Game)
                .WithMany(g => g.Bets)
                .OnDelete(DeleteBehavior.Restrict);



            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new ClothingConfiguration());
            builder.ApplyConfiguration(new AmmoBoxConfiguration());
            builder.ApplyConfiguration(new VendorConfiguration());
            builder.ApplyConfiguration(new MatchmakerConfiguration());
            builder.ApplyConfiguration(new PlayerClassConfiguration());
            builder.ApplyConfiguration(new PlayerConfiguration());
            builder.ApplyConfiguration(new WeaponConfiguration());
            builder.ApplyConfiguration(new TeamConfiguration());
            builder.ApplyConfiguration(new GameModeConfiguration());
            builder.ApplyConfiguration(new MapConfiguration());
            builder.ApplyConfiguration(new GameConfiguration());
            builder.ApplyConfiguration(new BetConfiguration());
            builder.ApplyConfiguration(new TeamRequestConfiguration());

            base.OnModelCreating(builder);
        }
    }
}