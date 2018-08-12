using System;

namespace FoosballAPI.Write.Commands
{
    public class CreateTeamCommand
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
    }
}
