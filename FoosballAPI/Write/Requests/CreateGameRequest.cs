using System;

namespace FoosballAPI.Write.Requests
{
    public class CreateGameRequest
    {
        public Guid GameId { get; set; }
        public Guid FirstTeamId { get; set; }
        public Guid SecondTeamId { get; set; }
    }
}
