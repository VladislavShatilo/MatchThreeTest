using System;
using VContainer;
using VContainer.Unity;

public class EnergyGainSystem : IEnergyGainSystem, IStartable, IDisposable
{
    private IGainEnergyUseCase useCase;
    private IEventBus eventBus;

    [Inject]
    private void Construct(IGainEnergyUseCase useCase, IEventBus eventBus)
    {
        this.useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public void Start()
    {
        eventBus.Subscribe<MatchesResolvedEvent>(OnMatchesResolved);
    }

    public void OnMatch(int count)
    {
        useCase.Execute(count);
    }

    public void Dispose()
    {
        eventBus.Unsubscribe<MatchesResolvedEvent>(OnMatchesResolved);
    }

    private void OnMatchesResolved(MatchesResolvedEvent evt)
    {
        if (evt.Matches == null)
            return;

        for (int i = 0; i < evt.Matches.Count; i++)
            OnMatch(evt.Matches[i].Cells.Count);
    }
}
