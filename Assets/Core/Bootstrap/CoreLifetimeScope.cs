using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class CoreLifetimeScope : LifetimeScope
{
    [SerializeField] private SwipeInputView swipeInput;
    [SerializeField] private GridView gridView;
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private SwapAnimator swapAnimator;

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterServices(builder);
    }

    private void RegisterServices(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<CoreEntryPoint>();

        builder.RegisterComponentInHierarchy<LoadMetaButtonView>();

        builder.Register<TileFactory>(Lifetime.Singleton)
           .As<ITileFactory>();

        builder.Register<GridService>(Lifetime.Singleton)
          .As<IGridService>();

        builder.Register<MatchFinderService>(Lifetime.Singleton)
        .As<IMatchFinderService>();

        builder.Register<SwapCommandHandler>(Lifetime.Singleton)
          .As<ISwapCommandHandler>();

        builder.Register<InputLockService>(Lifetime.Singleton)
         .As<IInputLockService>();

        builder.RegisterInstance(raycaster);
        builder.RegisterInstance(eventSystem);
        builder.RegisterComponentInHierarchy<GridView>();

        builder.Register<GridRaycaster>(Lifetime.Singleton)
        .As<IGridRaycaster>();

        builder.RegisterComponentInHierarchy<SwipeInputView>();
        builder.RegisterEntryPoint<SwapAnimationPresenter>().AsSelf();
        builder.RegisterComponentInHierarchy<SwapAnimator>();
    }
}