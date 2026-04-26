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


    [Inject]
    private void Construct( ILoadingScreenFactory loadingScreenFactory)
    {
        this.loadingScreenFactory = loadingScreenFactory ?? throw new ArgumentNullException(nameof(loadingScreenFactory));

    }
    public async UniTask Init(CancellationToken token)
    {
        var go = await loadingScreenFactory.Create(token);
        loadingScreenView = go.GetComponent<ILoadingScreenView>();
    }
    public async UniTask Show( CancellationToken token)
    {

        loadingScreenView.Show();
        await UniTask.Yield(token);
    }

    public async UniTask Hide(CancellationToken token)
    {
        loadingScreenView.Hide();

        await UniTask.Yield(token);
    }

}
