using VContainer;
using VContainer.Unity;
using System;
using UnityEngine;
public class SwapAnimationPresenter : IStartable, IDisposable
{
    private SwapAnimator animator;
    private IEventBus eventBus;
    private GridView gridView;

    [Inject]
    private void Construct(SwapAnimator animator,IEventBus eventBus, GridView gridView)
    {
        this.animator = animator != null ? animator : throw new ArgumentNullException(nameof(animator));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.gridView = gridView != null ? gridView : throw new ArgumentNullException(nameof(gridView));
    }
    public void Start()
    {
        eventBus.Subscribe<SwapRequestedEvent>(OnSwapRequested);
    }

    private async void OnSwapRequested(SwapRequestedEvent e)
    {
        await animator.Play(e.From, e.To, default);
        gridView.SwapViews(e.From, e.To);

        eventBus.Publish(new SwapAnimationCompletedEvent());
    }

    public void Dispose()
    {
        eventBus.Unsubscribe<SwapRequestedEvent>(OnSwapRequested);
    }
}