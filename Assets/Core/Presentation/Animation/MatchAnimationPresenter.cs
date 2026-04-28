using Cysharp.Threading.Tasks;
using System;
using VContainer.Unity;

public class MatchAnimationPresenter : IStartable, IDisposable
{
    private readonly IEventBus eventBus;
    private readonly GridView gridView;

    public MatchAnimationPresenter(IEventBus eventBus, GridView gridView)
    {
        this.eventBus = eventBus;
        this.gridView = gridView;

    }
    public void Start()
    {
        eventBus.Subscribe<ResolutionPlanCreatedEvent>(OnResolutionPlanCreated);
    }

    private async void OnResolutionPlanCreated(ResolutionPlanCreatedEvent evt)
    {
        await UniTask.Yield();
        await gridView.PlayPlan(evt.Plan);
    }

    public void Dispose()
    {
        eventBus.Unsubscribe<ResolutionPlanCreatedEvent>(OnResolutionPlanCreated);
    }

}