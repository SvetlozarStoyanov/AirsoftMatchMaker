using AirsoftMatchMaker.Infrastructure.Data.DataAccess.Repositories.Contracts;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.Repositories.Implementations;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.UnitOfWork;
using AirsoftMatchMaker.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly AirsoftMatchmakerDbContext context;

    #region Repository fields
    private IAmmoBoxRepository ammoBoxRepository;
    private IBetRepository betRepository;
    private IClothingRepository clothingRepository;
    private IGameRepository gameRepository;
    private IGameBetCreditsContainerRepository gameBetCreditsContainerRepository;
    private IGameModeRepository gameModeRepository;
    private IMapRepository mapRepository;
    private IMatchmakerRepository matchmakerRepository;
    private IPlayerRepository playerRepository;
    private IPlayerClassRepository playerClassRepository;
    private IRoleRequestRepository roleRequestRepository;
    private ITeamRepository teamRepository;
    private ITeamRequestRepository teamRequestRepository;
    private IUserRepository userRepository;
    private IVendorRepository vendorRepository;
    private IWeaponRepository weaponRepository;
    #endregion

    public UnitOfWork(AirsoftMatchmakerDbContext context)
    {
        this.context = context;
    }

    #region Repositories

    public IAmmoBoxRepository AmmoBoxRepository => ammoBoxRepository ??= new AmmoBoxRepository(context);
    public IBetRepository BetRepository => betRepository ??= new BetRepository(context);
    public IClothingRepository ClothingRepository => clothingRepository ??= new ClothingRepository(context);
    public IGameRepository GameRepository => gameRepository ??= new GameRepository(context);
    public IGameBetCreditsContainerRepository GameBetCreditsContainerRepository => gameBetCreditsContainerRepository ??= new GameBetCreditsContainerRepository(context);
    public IGameModeRepository GameModeRepository => gameModeRepository ??= new GameModeRepository(context);
    public IMapRepository MapRepository => mapRepository ??= new MapRepository(context);
    public IMatchmakerRepository MatchmakerRepository => matchmakerRepository ??= new MatchmakerRepository(context);
    public IPlayerRepository PlayerRepository => playerRepository ??= new PlayerRepository(context);
    public IPlayerClassRepository PlayerClassRepository => playerClassRepository ??= new PlayerClassRepository(context);
    public IRoleRequestRepository RoleRequestRepository => roleRequestRepository ??= new RoleRequestRepository(context);
    public ITeamRepository TeamRepository => teamRepository ??= new TeamRepository(context);
    public ITeamRequestRepository TeamRequestRepository => teamRequestRepository ??= new TeamRequestRepository(context);
    public IUserRepository UserRepository => userRepository ??= new UserRepository(context);
    public IVendorRepository VendorRepository => vendorRepository ??= new VendorRepository(context);
    public IWeaponRepository WeaponRepository => weaponRepository ??= new WeaponRepository(context);

    #endregion

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }

    public void Dispose()
    {
        context.Dispose();
    }
}
