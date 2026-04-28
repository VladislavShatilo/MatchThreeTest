using UnityEngine;
using VContainer;
using VContainer.Unity;

public class ProjectLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        RegisterServices(builder);
    }

    private void RegisterServices(IContainerBuilder builder)
    {
        builder.Register<EventBus>(Lifetime.Singleton).As<IEventBus>();
        builder.Register<EventAwaiter>(Lifetime.Singleton).As<IEventAwaiter>();

        builder.Register<GameStateRepository>(Lifetime.Singleton)
               .As<IGameStateRepository>();

        builder.Register<SceneLoader>(Lifetime.Singleton)
               .As<ISceneLoader>();

        builder.Register<LoadingScreenFactory>(Lifetime.Singleton)
               .As<ILoadingScreenFactory>();

        builder.RegisterEntryPoint<BootstrapEntryPoint>();


        builder.Register<SceneTransitionService>(Lifetime.Singleton);

        builder.Register<LoadingScreenService>(Lifetime.Singleton)
                .As<ILoadingScreenService>();

        builder.Register<ILoadMetaSceneUseCase, LoadMetaSceneUseCase>(Lifetime.Scoped);
        builder.Register<ILoadCoreSceneUseCase, LoadCoreSceneUseCase>(Lifetime.Scoped);





    }
}
