using System;

namespace FoosballAPI.Read.Results
{
    public class GameResult
    {
        public Guid GameId { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid FirstTeamId { get; set; }
        public string FirstTeamName { get; set; }
        public Guid SecondTeamId { get; set; }
        public string SecondTeamName { get; set; }
    }
}
