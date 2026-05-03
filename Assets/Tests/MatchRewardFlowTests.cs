using System.Collections.Generic;
using NUnit.Framework;

public class MatchRewardFlowTests
{
    [Test]
    public void MatchRewardService_PublishesMatchesResolvedEvent()
    {
        var eventBus = new EventBus();
        var service = new MatchRewardService();
        var received = default(MatchesResolvedEvent);
        var wasPublished = false;

        eventBus.Subscribe<MatchesResolvedEvent>(evt =>
        {
            received = evt;
            wasPublished = true;
        });

        InvokeConstruct(service, eventBus);

        var matches = new List<MatchGroup>
        {
            new(new List<GridPosition> { new(0, 0), new(1, 0), new(2, 0) })
        };

        service.Award(matches);

        Assert.That(wasPublished, Is.True);
        Assert.That(received.Matches, Is.SameAs(matches));
    }

    [Test]
    public void EnergyGainSystem_AddsEnergy_WhenMatchesResolvedEventIsPublished()
    {
        var eventBus = new EventBus();
        var state = new EnergyState();
        var useCase = new GainEnergyUseCase();
        var system = new EnergyGainSystem();

        InvokeConstruct(useCase, state);
        InvokeConstruct(system, useCase, eventBus);
        system.Start();

        try
        {
            eventBus.Publish(new MatchesResolvedEvent(new List<MatchGroup>
            {
                new(new List<GridPosition> { new(0, 0), new(1, 0), new(2, 0) }),
                new(new List<GridPosition> { new(0, 1), new(1, 1), new(2, 1), new(3, 1) })
            }));

            Assert.That(state.Current, Is.EqualTo(70));
        }
        finally
        {
            system.Dispose();
        }
    }

    private static void InvokeConstruct(MatchRewardService service, IEventBus eventBus)
    {
        var method = typeof(MatchRewardService).GetMethod("Construct", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        method.Invoke(service, new object[] { eventBus });
    }

    private static void InvokeConstruct(GainEnergyUseCase useCase, IEnergyStateWriter state)
    {
        var method = typeof(GainEnergyUseCase).GetMethod("Construct", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        method.Invoke(useCase, new object[] { state });
    }

    private static void InvokeConstruct(EnergyGainSystem system, IGainEnergyUseCase useCase, IEventBus eventBus)
    {
        var method = typeof(EnergyGainSystem).GetMethod("Construct", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        method.Invoke(system, new object[] { useCase, eventBus });
    }
}
