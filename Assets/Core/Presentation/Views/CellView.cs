using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CellView : MonoBehaviour
{
 
    private RectTransform rectTransform;
    private UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle handle;

    public void Initialize(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle handle)
    {
        this.handle = handle;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetPosition(Vector2 anchoredPosition)
    {
        EnsureRectTransform();
        rectTransform.anchoredPosition = anchoredPosition;
    }

    public async UniTask MoveTo(Vector2 anchoredPosition)
    {
        EnsureRectTransform();
        await rectTransform
            .DOAnchorPos(anchoredPosition, 0.25f)
            .SetEase(Ease.OutQuad)
            .AsyncWaitForCompletion();
    }

    public async UniTask DestroyAnim()
    {
        var tcs = new UniTaskCompletionSource();
        var completed = false;
        var tween = transform
            .DOScale(0, 0.15f)
            .SetEase(Ease.InBack);

        tween.OnComplete(() =>
        {
            if (this != null)
                Destroy(gameObject);

            completed = true;
            tcs.TrySetResult();
        });

        tween.OnKill(() =>
        {
            if (!completed)
                tcs.TrySetResult();
        });

        await tcs.Task;
    }

    private void EnsureRectTransform()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
    }
    private void OnDestroy()
    {
        if (handle.IsValid())
            Addressables.Release(handle);
    }
}