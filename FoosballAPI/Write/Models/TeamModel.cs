using System;
using System.Threading.Tasks;
using FoosballAPI.Database;
using FoosballAPI.Database.Entities;
using FoosballAPI.Infrastructure;
using FoosballAPI.Write.Commands;

namespace FoosballAPI.Write.Models
{
    public class TeamModel : ICommandHandler<CreateTeamCommand>
    {
        private readonly FoosballDbContext _dbContext;

        public TeamModel(FoosballDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Execute(CreateTeamCommand command)
        {
            ValidateCreateTeamCommand(command);

            _dbContext.Teams.Add(new TeamEntity
            {
                Id = command.TeamId,
                Name = command.Name
            });

            await _dbContext.SaveChangesAsync();
        }
        private static void ValidateCreateTeamCommand(CreateTeamCommand command)
        {
            if (command.TeamId == Guid.Empty)
                throw new Exception($"{nameof(command.TeamId)} cannot be empty!");

            if (string.IsNullOrEmpty(command.Name))
                throw new Exception($"{nameof(command.Name)} must be set!");
        }
    }
}
