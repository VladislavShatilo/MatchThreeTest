using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

public class SceneLoader : ISceneLoader
{
    private  LifetimeScope parentScope;
    private SceneInstance? currentScene;

    [Inject]
    private void Construct(LifetimeScope parentScope)
    {
        this.parentScope = parentScope;
    }
    public async UniTask LoadScene(string sceneKey, CancellationToken token)
    {
        LifetimeScope.EnqueueParent(parentScope);
        var handle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Additive,activateOnLoad: true);

        SceneInstance newScene;

        try
        {
            newScene = await handle.ToUniTask(cancellationToken: token);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to load scene: {sceneKey}\n{e}");
            throw;
        }

        SceneManager.SetActiveScene(newScene.Scene);

        if (currentScene.HasValue)
        {
            await Addressables.UnloadSceneAsync(currentScene.Value).ToUniTask(cancellationToken: token);
        }

        currentScene = newScene;
    }
}
    