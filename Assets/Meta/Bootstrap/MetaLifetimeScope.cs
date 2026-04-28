using VContainer;
using VContainer.Unity;

public class MetaLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        RegisterServices(builder);
    }

    private void RegisterServices(IContainerBuilder builder)
    {
        builder.RegisterComponentInHierarchy<LoadCoreButtonView>();
        builder.RegisterComponentInHierarchy<MetaEnergyView>();
        builder.RegisterComponentInHierarchy<TreeView>();


    }
}
