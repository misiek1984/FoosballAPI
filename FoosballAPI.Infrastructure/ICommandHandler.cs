using System.Threading.Tasks;

namespace FoosballAPI.Infrastructure
{
    public interface ICommandHandler<in TCommand>
    {
        Task Execute(TCommand command);
    }
}
