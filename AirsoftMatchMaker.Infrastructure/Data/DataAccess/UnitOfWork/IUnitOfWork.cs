using AirsoftMatchMaker.Infrastructure.Data.DataAccess.Repositories.Contracts;

namespace AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Repositories

        public IAmmoBoxRepository AmmoBoxRepository { get; }
        public IBetRepository BetRepository { get; }
        public IClothingRepository ClothingRepository { get; }
        public IGameRepository GameRepository { get; }
        public IGameBetCreditsContainerRepository GameBetCreditsContainerRepository { get; }
        public IGameModeRepository GameModeRepository { get; }
        public IMapRepository MapRepository { get; }
        public IMatchmakerRepository MatchmakerRepository { get; }
        public IPlayerRepository PlayerRepository { get; }
        public IPlayerClassRepository PlayerClassRepository { get; }
        public IRoleRequestRepository RoleRequestRepository { get; }
        public ITeamRepository TeamRepository { get; }
        public ITeamRequestRepository TeamRequestRepository { get; }
        public IUserRepository UserRepository { get; }
        public IVendorRepository VendorRepository { get; }
        public IWeaponRepository WeaponRepository { get; }

        #endregion

        public Task<int> SaveChangesAsync();
    }
}
