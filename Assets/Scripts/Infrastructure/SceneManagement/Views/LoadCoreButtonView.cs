using Cysharp.Threading.Tasks;
using System;
using VContainer;

public class LoadCoreButtonView : AsyncButtonView
{
    private ILoadCoreSceneUseCase loadCoreSceneUseCase;

    [Inject]
    private void Construct(ILoadCoreSceneUseCase loadCoreSceneUseCase)
    {
        this.loadCoreSceneUseCase = loadCoreSceneUseCase ?? throw new ArgumentNullException(nameof(loadCoreSceneUseCase));
    }

    protected override UniTask Execute() => loadCoreSceneUseCase.Execute();
}
