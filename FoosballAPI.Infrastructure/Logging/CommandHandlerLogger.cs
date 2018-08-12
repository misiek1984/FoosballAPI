using System.Threading.Tasks;
using Serilog;

namespace FoosballAPI.Infrastructure.Logging
{
    public class CommandHandlerLogger<TCommand> : ICommandHandler<TCommand>
    {
        private readonly ICommandHandler<TCommand> _handler;
        private readonly ILogger _logger;

        public CommandHandlerLogger(ICommandHandler<TCommand> handler, ILogger logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task Execute(TCommand command)
        {
            _logger.Information("Command = {Command}, Json = {@Json}", command.GetType().FullName, command);
            await _handler.Execute(command);
        }
    }
}
