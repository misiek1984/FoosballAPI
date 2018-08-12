using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoosballAPI.Infrastructure;
using FoosballAPI.Database;
using FoosballAPI.Read.Queries;
using FoosballAPI.Read.Results;

namespace FoosballAPI.Model.Read.Handlers
{
    public class TeamsQueryHandler : IQueryHandler<DefaultQuery, TeamResult>
    {
        private readonly FoosballDbContext _dbContext;

        public TeamsQueryHandler(FoosballDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<TeamResult>> Query(DefaultQuery query)
        {
            return _dbContext.Teams
                .Select(e =>
                    new TeamResult
                    {
                        Id = e.Id,
                        Name =  e.Name
                    }).
                OrderBy(t => t.Name);
        }
    }
}
