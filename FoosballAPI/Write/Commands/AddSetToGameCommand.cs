using System;

namespace FoosballAPI.Write.Commands
{
    public class AddSetToGameCommand
    {
        public Guid SetId { get; set; }
        public Guid GameId { get; set; }
    }
}
