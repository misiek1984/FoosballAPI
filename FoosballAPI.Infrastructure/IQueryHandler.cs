using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoosballAPI.Infrastructure
{
    public interface IQueryHandler<in TQuery, TResult>
    {
        Task<IEnumerable<TResult>> Query(TQuery query);
    }
}
