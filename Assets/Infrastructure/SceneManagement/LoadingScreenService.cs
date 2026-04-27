using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using VContainer;
public class LoadingScreenService : ILoadingScreenService
{
    private ILoadingScreenView loadingScreenView;
    private ILoadingScreenFactory loadingScreenFactory;

    private bool isInitialized;


    [Inject]
    private void Construct( ILoadingScreenFactory loadingScreenFactory)
    {
        this.loadingScreenFactory = loadingScreenFactory ?? throw new ArgumentNullException(nameof(loadingScreenFactory));

    }
    public async UniTask Initialize(CancellationToken token)
    {
        if (isInitialized) return;

        var go = await loadingScreenFactory.Create(token);
        loadingScreenView = go.GetComponent<ILoadingScreenView>();

        isInitialized = true;
    }


    public async UniTask Show(CancellationToken token)
    {
        await loadingScreenView.Show(token);
    }

    public async UniTask Hide(CancellationToken token)
    {
        await loadingScreenView.Hide(token);
    }

}
