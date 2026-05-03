using R3;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class TreePresenter : IStartable, IDisposable
{
    private IEnergyStateReader energyStateReader;
    private TreeView treeView;
    private ITreeShaderController treeShaderController;

    private IDisposable sub;

    [Inject]
    public void Construct(IEnergyStateReader energyStateReader, TreeView treeView, ITreeShaderController treeShaderController)
    {
        this.energyStateReader = energyStateReader ?? throw new ArgumentNullException(nameof(energyStateReader));
        this.treeView = treeView ?? throw new ArgumentNullException(nameof(treeView));
        this.treeShaderController = treeShaderController ?? throw new ArgumentNullException(nameof(treeShaderController));
    }

    public void Start()
    {
        sub = energyStateReader.Observe.Subscribe(OnEnergyChanged);
        OnEnergyChanged(energyStateReader.Current);
    }

    private void OnEnergyChanged(int energy)
    {
        var maxEnergy = Mathf.Max(1, treeView.MaxEnergy);
        float normalized = Mathf.Clamp01((float)energy / maxEnergy);

        treeShaderController.SetEnergy(normalized);
    }

    public void Dispose()
    {
        sub?.Dispose();
    }
}
