using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public abstract class AsyncButtonView : MonoBehaviour
{
    [SerializeField] private Button button;

    private bool isLoading;

    protected virtual void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (isLoading)
            return;

        ExecuteInternal().Forget();
    }

    private async UniTask ExecuteInternal()
    {
        try
        {
            isLoading = true;

            if (button != null)
                button.interactable = false;

            await Execute();
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
        finally
        {
            if (button != null)
                button.interactable = true;

            isLoading = false;
        }
    }

    protected abstract UniTask Execute();

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(OnClick);
    }
}