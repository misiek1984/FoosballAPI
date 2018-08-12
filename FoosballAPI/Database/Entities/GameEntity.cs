using System;
using System.Collections.Generic;
using FoosballAPI.Infrastructure;

namespace FoosballAPI.Database.Entities
{
    public class GameEntity : BaseEntity
    {
        public DateTime CreatedDate { get; set; }

        public TeamEntity FirstTeam { get; set; }

        public TeamEntity SecondTeam { get; set; }

        public ICollection<SetEntity> Sets { get; set; }

        public GameEntity()
        {
            Sets = new HashSet<SetEntity>();
        }
    }
}
