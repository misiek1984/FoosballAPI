using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosballAPI.Infrastructure;
using FoosballAPI.Database;
using FoosballAPI.Read.Queries;
using FoosballAPI.Read.Results;
using Microsoft.EntityFrameworkCore;

namespace FoosballAPI.Model.Read.Handlers
{
    public class GameQueryHandler : IQueryHandler<DefaultQuery, GameResult>
    {
        private readonly FoosballDbContext _dbContext;

        public GameQueryHandler(FoosballDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<GameResult>> Query(DefaultQuery query)
        {
            return _dbContext.Games
                .Include(x => x.Sets)
                .Include(x => x.FirstTeam)
                .Include(x => x.SecondTeam)
                .Select(e =>
                    new GameResult
                    {
                        GameId = e.Id,
                        CreatedDate = e.CreatedDate,
                        FirstTeamId = e.FirstTeam.Id,
                        FirstTeamName = e.FirstTeam.Name,
                        SecondTeamId = e.SecondTeam.Id,
                        SecondTeamName = e.SecondTeam.Name
                    }).
                OrderBy(t => t.CreatedDate);
        }
    }
}
