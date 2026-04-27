using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using VContainer;

public class SceneTransitionService
{
    private ISceneLoader sceneLoader;
    private ILoadingScreenService loadingScreenService;

    private bool isTransitioning;

    [Inject]
    private void Construct(ISceneLoader sceneLoader, ILoadingScreenService loadingScreenService)
    {
        this.sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
        this.loadingScreenService = loadingScreenService ?? throw new ArgumentNullException(nameof(loadingScreenService));
    }

    public async UniTask LoadScene(string sceneKey, CancellationToken token)
    {
        if (isTransitioning)
            throw new InvalidOperationException("Transition already in progress");

        isTransitioning = true;

        try
        {
            await loadingScreenService.Initialize(CancellationToken.None);

            await loadingScreenService.Show(CancellationToken.None);

            await UniTask.Yield(PlayerLoopTiming.Update);

            await sceneLoader.LoadScene(sceneKey, CancellationToken.None);

            await UniTask.Yield(PlayerLoopTiming.Update);

            await loadingScreenService.Hide(CancellationToken.None);
        }
        finally
        {
            isTransitioning = false;
        }
    }
}
