using Cysharp.Threading.Tasks;
using System;
using VContainer;

public class LoadMetaButtonView : AsyncButtonView
{
    private ILoadMetaSceneUseCase loadMetaSceneUseCase;

    [Inject]
    private void Construct(ILoadMetaSceneUseCase loadMetaSceneUseCase)
    {
        this.loadMetaSceneUseCase = loadMetaSceneUseCase ?? throw new ArgumentNullException(nameof(loadMetaSceneUseCase));
    }

    protected override UniTask Execute() => loadMetaSceneUseCase.Execute();
}
