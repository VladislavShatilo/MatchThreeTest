using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using VContainer;
using VContainer.Unity;

public class SceneLoader : ISceneLoader
{
    private LifetimeScope parentScope;
    private string persistentSceneName;
    private AsyncOperationHandle<SceneInstance>? currentSceneHandle;

    [Inject]
    private void Construct(LifetimeScope parentScope)
    {
        this.parentScope = parentScope;
        persistentSceneName = parentScope.gameObject.scene.name;
    }

    public async UniTask LoadScene(string sceneKey, CancellationToken token)
    {
        LifetimeScope.EnqueueParent(parentScope);
        var previousSceneHandle = currentSceneHandle;
        var handle = Addressables.LoadSceneAsync(sceneKey, LoadSceneMode.Additive, activateOnLoad: true);

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

        if (previousSceneHandle.HasValue && previousSceneHandle.Value.IsValid())
        {
            await Addressables.UnloadSceneAsync(previousSceneHandle.Value, autoReleaseHandle: true)
                .ToUniTask(cancellationToken: token);
        }

        currentSceneHandle = handle;
        await UnloadExtraScenes(newScene.Scene, token);
    }

    private async UniTask UnloadExtraScenes(Scene targetScene, CancellationToken token)
    {
        for (int i = SceneManager.sceneCount - 1; i >= 0; i--)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded)
                continue;

            if (scene.name == persistentSceneName)
                continue;

            if (scene.handle == targetScene.handle)
                continue;

            var unloadOperation = SceneManager.UnloadSceneAsync(scene);
            if (unloadOperation == null)
                continue;

            await unloadOperation.ToUniTask(cancellationToken: token);
        }
    }
}
    
