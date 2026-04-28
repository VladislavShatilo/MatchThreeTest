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
    [SerializeField] private TileDatabase tileDatabase;

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterServices(builder);
    }

    private void RegisterServices(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<CoreEntryPoint>();

        builder.RegisterComponentInHierarchy<LoadMetaButtonView>();


        builder.Register<GridService>(Lifetime.Singleton).As<IGridService>();

        builder.Register<MatchFinderService>(Lifetime.Singleton).As<IMatchFinderService>();

        builder.Register<SwapCommandHandler>(Lifetime.Singleton).As<ISwapCommandHandler>();

        builder.Register<InputLockService>(Lifetime.Singleton).As<IInputLockService>();

        builder.RegisterInstance(raycaster);
        builder.RegisterInstance(eventSystem);
        builder.RegisterComponentInHierarchy<GridView>();
        builder.RegisterComponentInHierarchy<EnergyView>();

        builder.Register<GridRaycaster>(Lifetime.Singleton).As<IGridRaycaster>();
        
        builder.RegisterComponentInHierarchy<SwipeInputView>();
        builder.RegisterEntryPoint<SwapAnimationPresenter>().AsSelf();
        builder.RegisterEntryPoint<MatchAnimationPresenter>().AsSelf();
        builder.RegisterComponentInHierarchy<SwapAnimator>();

        builder.Register<MatchResolutionService>(Lifetime.Singleton).As<IMatchResolutionService>();
        builder.Register<TileTypeGenerator>(Lifetime.Singleton).As<ITileTypeGenerator>();
        builder.Register<GridFactory>(Lifetime.Singleton).As<IGridFactory>();
        builder.Register<ITileFactory, TileFactory>(Lifetime.Singleton);
        builder.RegisterInstance(tileDatabase);
        builder.Register<EnergyGainSystem>(Lifetime.Singleton).As<IEnergyGainSystem>();

        
    }
}