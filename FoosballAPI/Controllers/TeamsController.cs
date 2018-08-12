using System.Collections.Generic;
using System.Threading.Tasks;
using FoosballAPI.Infrastructure;
using FoosballAPI.Read.Results;
using FoosballAPI.Write.Commands;
using FoosballAPI.Write.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FoosballAPI.Controllers
{
    [Route("api/[controller]")]
    public class TeamsController : Controller
    {
        private readonly ICommandHandler<CreateTeamCommand> _createTeamCommandHandler;
        private readonly IQueryHandler<DefaultQuery, TeamResult> _teamQueryHandler;

        public TeamsController(
            ICommandHandler<CreateTeamCommand> createTeamCommandHandler, 
            IQueryHandler<DefaultQuery, TeamResult> teamQueryHandler)
        {
            _createTeamCommandHandler = createTeamCommandHandler;
            _teamQueryHandler = teamQueryHandler;
        }

        [HttpGet]
        public async Task<IEnumerable<TeamResult>> Get(DefaultQuery query)
        {
            return await _teamQueryHandler.Query(query);
        }


        [HttpPost]
        public async Task Post([FromBody] CreateTeamRequest request)
        {
            await _createTeamCommandHandler.Execute(
                new CreateTeamCommand
                {
                    TeamId = request.TeamId,
                    Name = request.Name
                });
        }
    }
}
