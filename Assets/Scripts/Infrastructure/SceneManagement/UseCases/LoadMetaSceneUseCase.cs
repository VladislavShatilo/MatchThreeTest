using Cysharp.Threading.Tasks;
using System;
using VContainer;

public class LoadMetaSceneUseCase : ILoadMetaSceneUseCase
{
    private SceneTransitionService sceneTransitionService;

    [Inject]
    private void Construct(SceneTransitionService sceneTransitionService)
    {
        this.sceneTransitionService = sceneTransitionService ?? throw new ArgumentNullException(nameof(sceneTransitionService));
    }

    public UniTask Execute() => sceneTransitionService.LoadScene("MetaScene");
}
