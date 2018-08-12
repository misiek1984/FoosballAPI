using System;

namespace FoosballAPI.Infrastructure
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
