using System;

namespace FoosballAPI.Write.Requests
{
    public class AddGoalToSetRequest
    {
        public Guid SetId { get; set; }
        public bool FirstTeam { get; set; }
        public bool SecondTeam { get; set; }
    }
}
