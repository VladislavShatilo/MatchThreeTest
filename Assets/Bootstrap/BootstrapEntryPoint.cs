using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

public class BootstrapEntryPoint : IStartable
{

    private SceneTransitionService sceneTransitionService;


    [Inject]
    private void Construct(SceneTransitionService sceneTransitionService)
    {
        this.sceneTransitionService = sceneTransitionService ?? throw new ArgumentNullException(nameof(sceneTransitionService));
    }
    
    public async void Start()
    {
        await sceneTransitionService.LoadScene("CoreScene", default);
    }

}
