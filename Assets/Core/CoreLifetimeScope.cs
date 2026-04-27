using VContainer;
using VContainer.Unity;

public class CoreLifetimeScope : LifetimeScope
{
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


    }
}
