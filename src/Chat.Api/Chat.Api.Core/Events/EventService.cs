using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chat.Api.Core.Events
{
    public class EventService    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly Dictionary<string, List<Type>> Mappings = new Dictionary<string, List<Type>>();
        public EventService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public static void Subscribe<T, THandler>()
            where T : EventBase
            where THandler : IEventHandler<T>
        {
            var name = typeof(T).Name;

            if (!Mappings.ContainsKey(name))
            {
                Mappings.Add(name, new List<Type>());
            }

            Mappings[name].Add(typeof(THandler));
        }

        public static void Unsubscribe<T, THandler>()
            where T : EventBase
            where THandler : IEventHandler<T>
        {
            var name = typeof(T).Name;
            Mappings[name].Remove(typeof(THandler));

            if (Mappings[name].Count == 0)
            {
                Mappings.Remove(name);
            }
        }
        public async Task PublishAsync<T>(T o) where T : EventBase
        {
            var name = typeof(T).Name;

            if (Mappings.ContainsKey(name))
            {
                foreach (var handler in Mappings[name])
                {
                    var service = (IEventHandler<T>)_serviceProvider.GetService(handler);

                    if (service != null) await service.RunAsync(o);
                }
            }
        }
    }
}