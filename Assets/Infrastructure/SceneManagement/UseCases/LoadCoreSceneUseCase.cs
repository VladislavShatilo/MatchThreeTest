using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;
public class LoadCoreSceneUseCase : ILoadCoreSceneUseCase
{
    private SceneTransitionService sceneTransitionService;

    [Inject]
    private void Construct(SceneTransitionService sceneTransitionService)
    {
        this.sceneTransitionService = sceneTransitionService ?? throw new ArgumentNullException(nameof(sceneTransitionService));
    }
    public async UniTask Execute(CancellationToken token)
    {
        await sceneTransitionService.LoadScene("CoreScene", token);
    }
}
