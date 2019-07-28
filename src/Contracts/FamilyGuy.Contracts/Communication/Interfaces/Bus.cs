using Autofac;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FamilyGuy.Contracts.Communication.Interfaces
{
    public class Bus : ICommandBus, IEventBus, IQuery
    {
        private readonly IComponentContext _container;
        private readonly ILogger _logger;

        public Bus(IComponentContext container, ILoggerFactory loggerFactory)
        {
            _container = container;
            _logger = loggerFactory.CreateLogger(GetType());
        }

        public async Task Send<T>(T command) where T : ICommand
        {
            ICommandHandler<T> commandHandler;
            try
            {
                commandHandler = (ICommandHandler<T>)_container.Resolve(typeof(ICommandHandler<T>));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot create handler {typeof(ICommandHandler<T>)}", ex);
                throw;
            }

            await commandHandler.Handle(command);
        }

        public async Task Publish<T>(T @event) where T : IEvent
        {
            IEnumerable<IEventHandler<T>> eventHandlers =
                (IEnumerable<IEventHandler<T>>)_container.Resolve(typeof(IEnumerable<IEventHandler<T>>));

            foreach (IEventHandler<T> eventHandler in eventHandlers)
            {
                await eventHandler.Handle(@event);
            }
        }

        public TQueryResponse Query<TQueryResponse, TQueryRequest>(TQueryRequest request)
        {
            try
            {
                IQueryHandler<TQueryResponse, TQueryRequest> queryHandler = (IQueryHandler<TQueryResponse, TQueryRequest>)_container.Resolve(typeof(IQueryHandler<TQueryResponse, TQueryRequest>));
                return queryHandler.Handle(request);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Cannot find handler {typeof(IQueryHandler<TQueryResponse, TQueryRequest>)}", ex);
                throw;
            }
        }
    }
}
