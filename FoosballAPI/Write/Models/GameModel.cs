using System;
using System.Linq;
using System.Threading.Tasks;
using FoosballAPI.Database;
using FoosballAPI.Database.Entities;
using FoosballAPI.Infrastructure;
using FoosballAPI.Services;
using FoosballAPI.Write.Commands;
using Microsoft.EntityFrameworkCore;

namespace FoosballAPI.Write.Models
{
    public class GameModel :
        ICommandHandler<CreateGameCommand>,
        ICommandHandler<AddSetToGameCommand>,
        ICommandHandler<AddGoalToSetCommand>
    {
        private const int MaxSets = 3;
        private const int MaxGoals = 10;

        private readonly FoosballDbContext _dbContext;
        private readonly IDateTimeProvider _dateTimeProvider;

        public GameModel(FoosballDbContext dbContext, IDateTimeProvider dateTimeProvider)
        {
            _dbContext = dbContext;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task Execute(CreateGameCommand command)
        {
            var (firstTeam, secondTeam) = ValidateCreateGameCommand(command);

            _dbContext.Games.Add(new GameEntity
            {
                Id = command.GameId,
                FirstTeam = firstTeam,
                SecondTeam = secondTeam,
                CreatedDate = _dateTimeProvider.CurrentDataTime
            });

            await _dbContext.SaveChangesAsync();
        }
        private (TeamEntity firstTeam, TeamEntity secondTeam) ValidateCreateGameCommand(CreateGameCommand command)
        {
            if (command.GameId == Guid.Empty)
                throw new Exception($"{nameof(command.GameId)} cannot be empty!");

            if (command.FirstTeamId == command.SecondTeamId)
                throw new Exception($"The first team cannnot be the same as the seconde one!");

            var firstTeam = _dbContext.Teams.SingleOrDefault(t => t.Id == command.FirstTeamId);

            if (firstTeam == null)
                throw new Exception($"The team '{command.FirstTeamId}' was not found!");

            var secondTeam = _dbContext.Teams.SingleOrDefault(t => t.Id == command.SecondTeamId);

            if (secondTeam == null)
                throw new Exception($"The team '{command.SecondTeamId}' was not found!");

            return (firstTeam, secondTeam);
        }

        public async Task Execute(AddSetToGameCommand command)
        {
            var game = ValidateAddSetToGameCommand(command);

            game.Sets.Add(new SetEntity
            {
                Id = command.SetId,
                Parent =  game,
                ParentId = game.Id
            });

            await _dbContext.SaveChangesAsync();
        }
        private GameEntity ValidateAddSetToGameCommand(AddSetToGameCommand command)
        {
            if (command.GameId == Guid.Empty)
                throw new Exception($"{nameof(command.GameId)} cannot be empty!");

            if (command.SetId == Guid.Empty)
                throw new Exception($"{nameof(command.SetId)} cannot be empty!");

            var game = _dbContext.Games.Include(x => x.Sets).SingleOrDefault(t => t.Id == command.GameId);

            if (game == null)
                throw new Exception($"The team '{command.GameId}' was not found!");

            if (game.Sets.Count == MaxSets)
                throw new Exception($"The game '{command.GameId}' cannot have more than 3 sets!");

            if(game.Sets.Any() && !game.Sets.All(s => s.FirstTeamResult == MaxGoals || s.SecondTeamResult == MaxGoals))
                throw new Exception($"A new set cannot be added to the game '{command.GameId}' because the last one has not beed finished yet!");

            return game;
        }

        public async Task Execute(AddGoalToSetCommand command)
        {
            var set = ValidateAddGoalToSetCommand(command);

            if (command.FirstTeam)
                set.FirstTeamResult++;

            if (command.SecondTeam)
                set.SecondTeamResult++;

            await _dbContext.SaveChangesAsync();
        }
        private SetEntity ValidateAddGoalToSetCommand(AddGoalToSetCommand command)
        {
            if (command.GameId == Guid.Empty)
                throw new Exception($"{nameof(command.GameId)} cannot be empty!");

            if (command.SetId == Guid.Empty)
                throw new Exception($"{nameof(command.SetId)} cannot be empty!");

            var set = _dbContext.Sets.SingleOrDefault(s => s.Id == command.SetId && s.ParentId == command.GameId);

            if (set == null)
                throw new Exception($"The set '{command.SetId}' for the game '{command.GameId}' was not found!");

            if (set.FirstTeamResult == MaxGoals || set.SecondTeamResult == MaxGoals)
                throw new Exception($"The set '{command.SetId}' for the game '{command.GameId}' has already been finished");

            return set;
        }
    }
}
