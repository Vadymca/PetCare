namespace PetCare.Tests.Application.Events;

using MediatR;
using Moq;
using PetCare.Application.Abstractions.Events;
using PetCare.Domain.Events;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// Contains unit tests for <see cref="DomainEventDispatcher"/> to ensure events are dispatched correctly.
/// </summary>
public class DomainEventDispatcherTests
{
    /// <summary>
    /// Verifies that dispatching a single event invokes the mediator's <see cref="IMediator.Publish{T}"/> exactly once.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task DispatchAsync_Should_Invoke_Handler_Once()
    {
        var mediatorMock = new Mock<IMediator>();
        var dispatcher = new DomainEventDispatcher(mediatorMock.Object);

        var testEvent = new TestDomainEvent("Hello World");

        await dispatcher.DispatchAsync(new DomainEvent[] { testEvent });

        mediatorMock.Verify(
            m => m.Publish(
                It.Is<DomainEvent>(e => e as TestDomainEvent == testEvent),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies that dispatching multiple events invokes the mediator's <see cref="IMediator.Publish{T}"/> exactly once per event.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task DispatchAsync_Should_Invoke_Multiple_Events()
    {
        var mediatorMock = new Mock<IMediator>();
        var dispatcher = new DomainEventDispatcher(mediatorMock.Object);

        var events = new List<TestDomainEvent>
        {
            new("Event1"),
            new("Event2"),
        };

        await dispatcher.DispatchAsync(events);

        foreach (var ev in events)
        {
            mediatorMock.Verify(
                m => m.Publish(
                    It.Is<DomainEvent>(e => (e as TestDomainEvent) == ev),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

    /// <summary>
    /// Verifies that dispatching an empty list of events does not invoke the mediator and does not throw any exceptions.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task DispatchAsync_WithEmptyList_Should_NotThrow()
    {
        var mediatorMock = new Mock<IMediator>();
        var dispatcher = new DomainEventDispatcher(mediatorMock.Object);

        await dispatcher.DispatchAsync(new List<DomainEvent>());

        mediatorMock.Verify(
            m => m.Publish(It.IsAny<DomainEvent>(), It.IsAny<CancellationToken>()),
            Times.Never);
    }
}

/// <summary>
/// Represents a test domain event used for unit testing <see cref="DomainEventDispatcher"/>.
/// </summary>
/// <param name="message">The message carried by the event.</param>
public sealed record TestDomainEvent(string message) : DomainEvent;
