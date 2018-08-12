using System;
using FoosballAPI.Infrastructure;

namespace FoosballAPI.Database.Entities
{
    public class SetEntity : BaseEntity
    {
        public GameEntity Parent { get; set; }

        public Guid ParentId { get; set; }

        public int FirstTeamResult { get; set; }

        public int SecondTeamResult { get; set; }
    }
}
