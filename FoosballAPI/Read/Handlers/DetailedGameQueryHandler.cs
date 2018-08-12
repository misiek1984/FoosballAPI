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
    public class DetailedGameQueryHandler : IQueryHandler<GameQuery, DetailedGameResult>
    {
        private readonly FoosballDbContext _dbContext;

        public DetailedGameQueryHandler(FoosballDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<DetailedGameResult>> Query(GameQuery query)
        {
            return _dbContext.Games
                .Where(e => e.Id == query.GameId)
                .Include(x => x.Sets)
                .Include(x => x.FirstTeam)
                .Include(x => x.SecondTeam)
                .Select(e =>
                    new DetailedGameResult
                    {
                        GameId = e.Id,
                        CreatedDate = e.CreatedDate,
                        FirstTeamId = e.FirstTeam.Id,
                        FirstTeamName = e.FirstTeam.Name,
                        SecondTeamId = e.SecondTeam.Id,
                        SecondTeamName = e.SecondTeam.Name,
                        Sets = e.Sets.Select(s => new SetResult
                        {
                            SetId = s.Id,
                            FirstTeamResult = s.FirstTeamResult,
                            SecondTeamResult = s.SecondTeamResult
                        }).ToArray()
                    })
                .OrderBy(t => t.CreatedDate);
        }
    }
}
