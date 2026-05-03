using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CellView : MonoBehaviour
{
    private RectTransform rectTransform;

    private bool isAddressableInstance;
    private bool isReleased;

    private Tween currentTween;

    public void MarkAsAddressableInstance()
    {
        isAddressableInstance = true;
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

    public async UniTask MoveTo(Vector2 anchoredPosition, CancellationToken ct)
    {
        EnsureRectTransform();

        ct.ThrowIfCancellationRequested();

        KillCurrentTween();

        var tcs = new UniTaskCompletionSource();

        currentTween = DOTween
            .To(
                () => rectTransform.anchoredPosition,
                value => rectTransform.anchoredPosition = value,
                anchoredPosition,
                0.25f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => tcs.TrySetResult())
            .OnKill(() => tcs.TrySetResult());

        using (ct.Register(() =>
        {
            KillCurrentTween();
            tcs.TrySetCanceled(ct);
        }))
        {
            await tcs.Task;
        }

        currentTween = null;
    }


    public async UniTask DestroyAnim(CancellationToken ct)
    {
        if (isReleased || this == null)
            return;

        EnsureRectTransform();

        ct.ThrowIfCancellationRequested();

        KillCurrentTween();

        var tcs = new UniTaskCompletionSource();
        bool finished = false;

        currentTween = transform
            .DOScale(0f, 0.15f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                finished = true;
                tcs.TrySetResult();
            })
            .OnKill(() =>
            {
                if (!finished)
                    tcs.TrySetResult();
            });

        try
        {
            await tcs.Task.AttachExternalCancellation(ct);
        }
        catch (OperationCanceledException)
        {
            KillCurrentTween();
            throw;
        }
        finally
        {
            currentTween = null;
        }
    }

  
    public void Release()
    {
        if (isReleased || this == null)
            return;

        isReleased = true;

        KillCurrentTween();

        if (isAddressableInstance)
        {
            Addressables.ReleaseInstance(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (!isReleased && isAddressableInstance)
        {
            isReleased = true;
            Addressables.ReleaseInstance(gameObject);
        }
    }

 
    private void KillCurrentTween()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill(false);
            currentTween = null;
        }
    }

    private void EnsureRectTransform()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();
    }
}
