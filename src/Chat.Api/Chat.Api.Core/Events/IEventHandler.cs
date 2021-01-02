using System.Threading.Tasks;

namespace Chat.Api.Core.Events
{
    public interface IEventHandler<in T> where T : IDomainEvent
    {
        Task RunAsync(T obj);
    }
}