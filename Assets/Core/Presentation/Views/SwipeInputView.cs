using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;


public class SwipeInputView : MonoBehaviour
{ 
    private Vector2 startPointerPos; 
    private Vector2Int startCell; 
    private bool dragging; 
    private const float SwipeThreshold = 30f;

    private ISwapCommandHandler swapHandler;
    private IInputLockService inputLock;
    private IGridRaycaster gridRaycaster; 
    private GridView gridView; 
    private IGridService gridService;

    [Inject]
    public void Construct(ISwapCommandHandler swapHandler, IInputLockService inputLock, IGridRaycaster gridRaycaster, GridView gridView, IGridService gridService)
    {
        this.swapHandler = swapHandler ?? throw new ArgumentNullException(nameof(swapHandler));
        this.inputLock = inputLock ?? throw new ArgumentNullException(nameof(inputLock)); 
        this.gridRaycaster = gridRaycaster ?? throw new ArgumentNullException(nameof(gridRaycaster)); 
        this.gridView = gridView ?? throw new ArgumentNullException(nameof(gridView));
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
    }
    private void Update()
    {
        if (inputLock.IsLocked)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (!gridRaycaster.TryGetCellView(Input.mousePosition, out var cellView))
                return;

            if (!gridView.TryGetGridPosition(cellView, out startCell))
                return;

            startPointerPos = Input.mousePosition; 
            dragging = true;
        }
        if (Input.GetMouseButtonUp(0) && dragging) 
        {
            dragging = false; 

            Vector2 delta = (Vector2)Input.mousePosition - startPointerPos; 
            if (delta.magnitude < SwipeThreshold) 
                return;

            var direction = GetSwipeDirection(delta);
            var targetCell = startCell + direction;

            if (!GridRules.IsValidPosition(targetCell, gridService.Grid.Width, gridService.Grid.Height))
                return; 

            var command = new SwapCommand(new SwapRequest(startCell, targetCell)); 
            swapHandler.Execute(command, this.GetCancellationTokenOnDestroy()).Forget(); 
        }
    }
    private Vector2Int GetSwipeDirection(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y)) 
        { 
            return delta.x > 0 ? Vector2Int.right : Vector2Int.left; 
        } 
        else 
        { 
            return delta.y > 0 ? Vector2Int.down : Vector2Int.up;
        }
    }
}