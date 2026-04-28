using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class GridView : MonoBehaviour
{
    private IGridService gridService;
    private ITileFactory tileFactory;
    private SwapAnimator swapAnimator;

    private readonly Dictionary<Vector2Int, CellView> views = new();
    private Dictionary<CellView, Vector2Int> reverseViews = new();

    [Inject]
    private void Construct(IGridService gridService, ITileFactory tileFactory, SwapAnimator swapAnimator)
    {
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
        this.tileFactory = tileFactory ?? throw new ArgumentNullException(nameof(tileFactory));
        this.swapAnimator = swapAnimator ?? throw new ArgumentNullException(nameof(swapAnimator));
    }

    private async void Start()
    {
        Build().Forget();
    }

    private async UniTask Build()
    {
        var grid = gridService.Grid;
        for (int x = 0; x < grid.Width; x++) 
        { 
            for (int y = 0; y < grid.Height; y++) 
            { 
                var cell = grid.Get(x, y);
                var tile = await tileFactory.Create(cell.Type);
                tile.transform.SetParent(transform, false); 
                if (tile.TryGetComponent<CellView>(out var cellView))
                { 
                    var pos = new Vector2Int(cell.X, cell.Y); 
                    cellView.SetPosition(pos); views[pos] = cellView;
                    reverseViews[cellView] = pos; 
                } 
            } 
        }
        swapAnimator.Initialize(views); 
    }

    public void SwapViews(Vector2Int from, Vector2Int to) 
    { 
        var a = views[from];
        var b = views[to];

        views[from] = b; 
        views[to] = a;

        reverseViews[a] = to; 
        reverseViews[b] = from;

        a.SetPosition(to);
        b.SetPosition(from); 
    }

    public Vector2Int GetGridPosition(CellView view)
    {
        return reverseViews[view];
    }
}