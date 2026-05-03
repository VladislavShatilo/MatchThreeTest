using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class BootstrapEntryPoint : IAsyncStartable
{

    private SceneTransitionService sceneTransitionService;


    [Inject]
    private void Construct(SceneTransitionService sceneTransitionService)
    {
        this.sceneTransitionService = sceneTransitionService ?? throw new ArgumentNullException(nameof(sceneTransitionService));
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        try
        {
            await sceneTransitionService.LoadScene("CoreScene");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
