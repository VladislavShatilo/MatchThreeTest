using System;
using UnityEngine;
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

        builder.RegisterComponentInHierarchy<TreeView>();

        builder.RegisterComponentInHierarchy<EnergyView>();

        builder.Register<TreeShaderController>(Lifetime.Singleton).As<ITreeShaderController>();

        builder.Register<TreePresenter>(Lifetime.Singleton).As<IStartable>().As<IDisposable>();
    }
}
