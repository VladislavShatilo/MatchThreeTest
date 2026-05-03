using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadingScreenFactory : ILoadingScreenFactory
{
    public async UniTask<GameObject> Create(CancellationToken token)
    {
        var instance = await Addressables.InstantiateAsync("LoadingScreen").ToUniTask(cancellationToken: token);
        if (instance.TryGetComponent<LoadingScreenView>(out var view))
            view.MarkAsAddressableInstance();
        return instance;
    }
}
