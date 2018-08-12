using System;
using FoosballAPI.Infrastructure;

namespace FoosballAPI.Read.Queries
{
    public class GameQuery : DefaultQuery
    {
        public Guid GameId { get; set; }
    }
}
