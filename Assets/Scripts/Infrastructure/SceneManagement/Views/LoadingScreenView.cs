using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LoadingScreenView : MonoBehaviour, ILoadingScreenView
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 0.3f;
    private bool isAddressableInstance;
    private bool isReleased;

    public void MarkAsAddressableInstance()
    {
        isAddressableInstance = true;
    }

    public async UniTask Show(CancellationToken token)
    {
        gameObject.SetActive(true);

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        await Fade(0, 1, token);
    }

    public async UniTask Hide(CancellationToken token)
    {
        await Fade(1, 0, token);

        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        gameObject.SetActive(false);
    }

    private async UniTask Fade(float from, float to, CancellationToken token)
    {
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, time / duration);

            await UniTask.Yield(token);
        }

        canvasGroup.alpha = to;
    }

    private void OnDestroy()
    {
        if (!isAddressableInstance || isReleased)
            return;

        isReleased = true;
        Addressables.ReleaseInstance(gameObject);
    }
}
