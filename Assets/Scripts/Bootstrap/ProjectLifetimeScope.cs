using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

public class ProjectLifetimeScope : LifetimeScope
{
    [SerializeField] private EventSystem eventSystem;

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterServices(builder);
    }

    private void RegisterServices(IContainerBuilder builder)
    {
        builder.Register<EventBus>(Lifetime.Singleton).As<IEventBus>();

        builder.Register<SceneLoader>(Lifetime.Singleton).As<ISceneLoader>();

        builder.Register<LoadingScreenFactory>(Lifetime.Singleton).As<ILoadingScreenFactory>();

        builder.RegisterEntryPoint<BootstrapEntryPoint>();

        builder.Register<SceneTransitionService>(Lifetime.Singleton);

        builder.Register<LoadingScreenService>(Lifetime.Singleton).As<ILoadingScreenService>();

        builder.Register<LoadMetaSceneUseCase>(Lifetime.Scoped).As<ILoadMetaSceneUseCase>();

        builder.Register<LoadCoreSceneUseCase>(Lifetime.Scoped).As<ILoadCoreSceneUseCase>();

        builder.RegisterInstance(eventSystem);

        builder.Register<EnergyState>(Lifetime.Singleton).As<IEnergyStateReader>().As<IEnergyStateWriter>();
    }
}
