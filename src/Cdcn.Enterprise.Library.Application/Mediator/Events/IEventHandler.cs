﻿using MediatR;

namespace Cdcn.Enterprise.Library.Application.Mediator.Events
{
    /// <summary>
    /// Represents the event handler interface.
    /// </summary>
    /// <typeparam name="TEvent">The event type.</typeparam>
    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
        where TEvent : INotification
    {
    }
}