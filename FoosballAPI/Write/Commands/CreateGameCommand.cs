using System;

namespace FoosballAPI.Write.Commands
{
    public class CreateGameCommand
    {
        public Guid GameId { get; set; }
        public Guid FirstTeamId { get; set; }
        public Guid SecondTeamId { get; set; }
    }
}
