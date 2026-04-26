using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

public class SceneTransitionService
{
    private ISceneLoader sceneLoader;
    private ILoadingScreenService loadingScreenService;

    [Inject]
    private void Construct(ISceneLoader sceneLoader, ILoadingScreenService loadingScreenService)
    {
        this.sceneLoader = sceneLoader ?? throw new ArgumentNullException(nameof(sceneLoader));
        this.loadingScreenService = loadingScreenService ?? throw new ArgumentNullException(nameof(loadingScreenService));
    }

    public async UniTask LoadScene(string sceneKey, CancellationToken token)
    {
        await loadingScreenService.Init(token);

        await loadingScreenService.Show(token);

        await sceneLoader.LoadScene(sceneKey, token);
        
    }
}
