using System.Collections.Generic;
using System.Threading.Tasks;
using Serilog;

namespace FoosballAPI.Infrastructure.Logging
{
    public class QueryHandlerLogger<TQuery, TResult> : IQueryHandler<TQuery, TResult>
    {
        private readonly IQueryHandler<TQuery, TResult> _handler;
        private readonly ILogger _logger;

        public QueryHandlerLogger(IQueryHandler<TQuery, TResult> handler, ILogger logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task<IEnumerable<TResult>> Query(TQuery query)
        {
            _logger.Information("Query = {Query}, Json = {@Json}", query.GetType().FullName, query);
            return await _handler.Query(query);
        }
    }
}
