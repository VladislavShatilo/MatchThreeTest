using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer;

public class SceneTransitionService
{
    private ISceneLoader sceneLoader;
    private ILoadingScreenService loadingScreenService;

    private bool isTransitioning;
    private CancellationTokenSource transitionCts;
    private string currentSceneKey;
    private string requestedSceneKey;

    [Inject]
    private void Construct(ISceneLoader sceneLoader, ILoadingScreenService loadingScreenService)
    {
        this.sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
        this.loadingScreenService = loadingScreenService ?? throw new ArgumentNullException(nameof(loadingScreenService));
    }

    public async UniTask LoadScene(string sceneKey)
    {
        if (string.IsNullOrWhiteSpace(sceneKey))
            throw new ArgumentException("Scene key must not be empty.", nameof(sceneKey));

        if (string.Equals(currentSceneKey, sceneKey, StringComparison.Ordinal))
            return;

        if (isTransitioning && string.Equals(requestedSceneKey, sceneKey, StringComparison.Ordinal))
            return;

        if (isTransitioning)
            throw new InvalidOperationException("Transition already in progress");

        isTransitioning = true;
        requestedSceneKey = sceneKey;

        transitionCts?.Cancel();
        transitionCts?.Dispose();
        transitionCts = new CancellationTokenSource();

        var token = transitionCts.Token;

        try
        {
            await loadingScreenService.Initialize(token);
            await loadingScreenService.Show(token);

            await UniTask.Yield(PlayerLoopTiming.Update, token);

            await sceneLoader.LoadScene(sceneKey, token);
            currentSceneKey = sceneKey;
        }
        catch (OperationCanceledException)
        {
        }
        finally
        {
            try
            {
                await loadingScreenService.Hide(CancellationToken.None);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            isTransitioning = false;
            requestedSceneKey = null;
        }
    }
}
