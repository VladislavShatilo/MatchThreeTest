using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadingScreenFactory : ILoadingScreenFactory
{
    public async UniTask<GameObject> Create(CancellationToken token)
    {
        var prefab = await Addressables.LoadAssetAsync<GameObject>("LoadingScreen").ToUniTask(cancellationToken: token);

        var instance = Object.Instantiate(prefab);

        return instance;
    }
}
