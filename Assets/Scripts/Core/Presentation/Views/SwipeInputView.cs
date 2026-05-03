using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

public class SwipeInputView : MonoBehaviour
{
    private GridPosition startCell;
    private bool hasActiveSwipe;
    private readonly SwipeDetector swipeDetector = new();

    private IInputLockService inputLock;
    private IGridRaycaster gridRaycaster;
    private ISwipeInputHandler swipeInputHandler;

    [Inject]
    public void Construct(
        IInputLockService inputLock,
        IGridRaycaster gridRaycaster,
        ISwipeInputHandler swipeInputHandler)
    {
        this.inputLock = inputLock ?? throw new ArgumentNullException(nameof(inputLock));
        this.gridRaycaster = gridRaycaster ?? throw new ArgumentNullException(nameof(gridRaycaster));
        this.swipeInputHandler = swipeInputHandler ?? throw new ArgumentNullException(nameof(swipeInputHandler));
    }

    private void Update()
    {
        if (inputLock.IsLocked)
            return;

        var pointerPosition = ToScreenPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (!gridRaycaster.TryGetGridPosition(pointerPosition, out startCell))
                return;

            hasActiveSwipe = true;
            swipeDetector.TryProcess(down: true, up: false, pointerPosition, out _);
        }

        if (!Input.GetMouseButtonUp(0) || !hasActiveSwipe)
            return;

        hasActiveSwipe = false;

        if (!swipeDetector.TryProcess(down: false, up: true, pointerPosition, out var direction))
            return;

        HandleSwipeAsync(new SwipeIntent(startCell, direction)).Forget();
    }

    private static ScreenPoint ToScreenPoint(Vector3 position)
    {
        return new ScreenPoint(position.x, position.y);
    }

    private async UniTaskVoid HandleSwipeAsync(SwipeIntent intent)
    {
        try
        {
            await swipeInputHandler.HandleSwipe(intent, this.GetCancellationTokenOnDestroy());
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}
