using System;
using UnityEngine;
using VContainer;

public class TreeShaderController : ITreeShaderController
{
    private static readonly int EnergyId = Shader.PropertyToID("_Energy");

    private TreeView treeView;
    private MaterialPropertyBlock propertyBlock;

    [Inject]
    public void Construct(TreeView treeView)
    {
        this.treeView = treeView ?? throw new ArgumentNullException(nameof(treeView));
        propertyBlock = new MaterialPropertyBlock();
    }

    public void SetEnergy(float normalizedEnergy)
    {
        var renderer = treeView.TargetRenderer;
        if (renderer == null)
            return;

        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat(EnergyId, normalizedEnergy);
        renderer.SetPropertyBlock(propertyBlock);
    }
}
