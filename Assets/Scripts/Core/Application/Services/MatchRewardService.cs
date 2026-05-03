using System;
using System.Collections.Generic;
using VContainer;

public class MatchRewardService : IMatchRewardService
{
    private IEventBus eventBus;

    [Inject]
    private void Construct(IEventBus eventBus)
    {
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
    }

    public void Award(IReadOnlyList<MatchGroup> matches)
    {
        if (matches == null)
            throw new ArgumentNullException(nameof(matches));

        if (matches.Count == 0)
            return;

        eventBus.Publish(new MatchesResolvedEvent(matches));
    }
}
