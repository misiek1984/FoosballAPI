using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoosballAPI.Infrastructure;
using FoosballAPI.Read.Queries;
using FoosballAPI.Read.Results;
using FoosballAPI.Write.Commands;
using FoosballAPI.Write.Requests;
using Microsoft.AspNetCore.Mvc;

namespace FoosballAPI.Controllers
{
    [Route("api/[controller]")]
    public class GamesController : Controller
    {
        private readonly ICommandHandler<CreateGameCommand> _createGameCommandHandler;
        private readonly ICommandHandler<AddSetToGameCommand> _addSetToGameCommand;
        private readonly ICommandHandler<AddGoalToSetCommand> _addGoalToSetCommand;
        private readonly IQueryHandler<DefaultQuery, GameResult> _gameQueryHandler;
        private readonly IQueryHandler<GameQuery, DetailedGameResult> _detailedGameQueryHandler;

        public GamesController(
            IQueryHandler<DefaultQuery, GameResult> gameQueryHandler,
            IQueryHandler<GameQuery, DetailedGameResult> detailedGameQueryHandler,
            ICommandHandler<CreateGameCommand> createGameCommandHandler, 
            ICommandHandler<AddSetToGameCommand> addSetToGameCommand, 
            ICommandHandler<AddGoalToSetCommand> addGoalToSetCommand)
        {
            _createGameCommandHandler = createGameCommandHandler;
            _detailedGameQueryHandler = detailedGameQueryHandler;
            _gameQueryHandler = gameQueryHandler;
            _addSetToGameCommand = addSetToGameCommand;
            _addGoalToSetCommand = addGoalToSetCommand;
            _addSetToGameCommand = addSetToGameCommand;
        }

        [HttpGet]
        public async Task<IEnumerable<GameResult>> Get(DefaultQuery query)
        {
            return await _gameQueryHandler.Query(query);
        }

        [HttpGet("{gameId}")]
        public async Task<IEnumerable<GameResult>> Get(Guid gameId)
        {
            return await _detailedGameQueryHandler.Query(new GameQuery { GameId = gameId });
        }


        [HttpPost]
        public async Task Post([FromBody] CreateGameRequest request)
        {
            await _createGameCommandHandler.Execute(
                new CreateGameCommand
                {
                    GameId = request.GameId,
                    FirstTeamId = request.FirstTeamId,
                    SecondTeamId = request.SecondTeamId
                });
        }

        [HttpPut("AddSetToGame/{gameId}")]
        public async Task AddSetToGame(Guid gameId, [FromBody] AddSetToGameRequest request)
        {
            await _addSetToGameCommand.Execute(
                new AddSetToGameCommand
                {
                    GameId = gameId,
                    SetId = request.SetId,
                });
        }

        [HttpPut("AddGoalToSet/{gameId}")]
        public async Task AddGoalToSet(Guid gameId, [FromBody] AddGoalToSetRequest request)
        {
            await _addGoalToSetCommand.Execute(
                new AddGoalToSetCommand
                {
                    GameId = gameId,
                    SetId = request.SetId,
                    FirstTeam = request.FirstTeam,
                    SecondTeam = request.SecondTeam,
                });
        }
    }
}
