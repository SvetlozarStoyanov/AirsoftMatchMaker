using AirsoftMatchMaker.Infrastructure.Data.DataAccess.BaseRepository;
using AirsoftMatchMaker.Infrastructure.Data.DataAccess.Repositories.Contracts;
using AirsoftMatchMaker.Infrastructure.Data.Entities;

namespace AirsoftMatchMaker.Infrastructure.Data.DataAccess.Repositories.Implementations
{
    public class GameModeRepository : BaseRepository<GameMode>, IGameModeRepository
    {
        public GameModeRepository(AirsoftMatchmakerDbContext context) : base(context)
        {
        }
    }
}
