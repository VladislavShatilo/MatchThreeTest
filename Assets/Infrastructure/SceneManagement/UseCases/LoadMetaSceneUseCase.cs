using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using VContainer;

public class LoadMetaSceneUseCase : ILoadMetaSceneUseCase
{
    private SceneTransitionService sceneTransitionService;

    [Inject]
    private void Construct(SceneTransitionService sceneTransitionService)
    {
        this.sceneTransitionService = sceneTransitionService ?? throw new ArgumentNullException(nameof(sceneTransitionService));
    }

    public async UniTask Execute(CancellationToken token)
    {
        await sceneTransitionService.LoadScene("MetaScene", token);
    }
}