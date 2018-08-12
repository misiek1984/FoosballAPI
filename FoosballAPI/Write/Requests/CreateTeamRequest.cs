using System;

namespace FoosballAPI.Write.Requests
{
    public class CreateTeamRequest
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
    }
}
