using System;

namespace FoosballAPI.Write.Commands
{
    public class AddGoalToSetCommand
    {
        public Guid GameId { get; set; }
        public Guid SetId { get; set; }
        public bool FirstTeam { get; set; }
        public bool SecondTeam { get; set; }
    }
}
