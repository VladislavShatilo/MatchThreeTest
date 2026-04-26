using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading;
using VContainer;
using System;
using UnityEngine;

public class SceneLoader : ISceneLoader
{
    public async UniTask LoadScene(string sceneKey, CancellationToken token)
    {
        var handle = Addressables.LoadSceneAsync(sceneKey);
        await handle.ToUniTask(cancellationToken: token);
    }
}
    