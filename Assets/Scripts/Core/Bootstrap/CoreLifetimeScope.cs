using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

public class CoreLifetimeScope : LifetimeScope
{
    [SerializeField] private GraphicRaycaster raycaster;
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

        builder.Register<BoardStateService>(Lifetime.Singleton).As<IBoardStateService>();

        builder.Register<MatchRewardService>(Lifetime.Singleton).As<IMatchRewardService>();

        builder.Register<ResolutionStepApplier>(Lifetime.Singleton).As<IResolutionStepApplier>();

        builder.Register<SwipeInputHandler>(Lifetime.Singleton).As<ISwipeInputHandler>();

        builder.Register<SwapCommandFactory>(Lifetime.Singleton).As<ISwapCommandFactory>();

        builder.Register<SwapMoveService>(Lifetime.Singleton).As<ISwapMoveService>();

        builder.Register<InputLockService>(Lifetime.Singleton).As<IInputLockService>();

        builder.Register<GridAnimationPlayer>(Lifetime.Singleton).As<IBoardAnimationPort>();

        builder.RegisterInstance(raycaster);

        builder.RegisterComponentInHierarchy<EnergyView>();

        builder.RegisterComponentInHierarchy<GridView>();

        builder.Register<GridRaycaster>(Lifetime.Singleton).As<IGridRaycaster>();

        builder.RegisterComponentInHierarchy<SwipeInputView>();

        builder.Register<MatchResolutionEngine>(Lifetime.Singleton).As<IMatchResolutionEngine>();

        builder.Register<AnimatedMatchResolutionService>(Lifetime.Singleton).As<IMatchResolutionService>();

        builder.Register<TileTypeGenerator>(Lifetime.Singleton).As<ITileTypeGenerator>();

        builder.Register<GridFactory>(Lifetime.Singleton).As<IGridFactory>();

        builder.Register<ITileFactory, TileFactory>(Lifetime.Singleton);

        builder.RegisterInstance(tileDatabase);
        
        builder.Register<GainEnergyUseCase>(Lifetime.Singleton).As<IGainEnergyUseCase>();

        builder.Register<EnergyGainSystem>(Lifetime.Singleton).As<IEnergyGainSystem>().As<IStartable>().As<IDisposable>();

    }
}
