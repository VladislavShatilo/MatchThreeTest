using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using VContainer;

public class GridView : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector2 spacing = Vector2.zero;
    [SerializeField] private Vector2 gridOrigin = Vector2.zero;

    private ITileFactory tileFactory;
    private readonly Dictionary<GridPosition, CellView> views = new();

    [Inject]
    private void Construct(ITileFactory tileFactory)
    {
        this.tileFactory = tileFactory ?? throw new ArgumentNullException(nameof(tileFactory));
    }

    public bool TryGetGridPosition(CellView view, out GridPosition position)
    {
        foreach (var kvp in views)
        {
            if (kvp.Value == view)
            {
                position = kvp.Key;
                return true;
            }
        }

        position = default;
        return false;
    }

    public async UniTask Render(GridSnapshot grid, CancellationToken ct)
    {
        try
        {
            ct.ThrowIfCancellationRequested();
            ReleaseAllViews();

            for (int x = 0; x < grid.Width; x++)
            {
                for (int y = 0; y < grid.Height; y++)
                {
                    ct.ThrowIfCancellationRequested();

                    var cell = grid.Get(x, y);
                    if (cell == null)
                        continue;

                    var view = await tileFactory.Create(cell.Type, ct);
                    if (ct.IsCancellationRequested)
                    {
                        view.Release();
                        ct.ThrowIfCancellationRequested();
                    }

                    view.transform.SetParent(transform, false);

                    var position = new GridPosition(x, y);
                    views[position] = view;

                    view.SetPosition(GridToUI(position));
                }
            }
        }
        catch (OperationCanceledException)
        {
            ReleaseAllViews();
            throw;
        }
    }

   
    public async UniTask AnimateResolutionStep(ResolutionStepPlan plan, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var tasks = new List<UniTask>();

        var removed = new List<CellView>();

        foreach (var pos in plan.RemovedCells)
        {
            ct.ThrowIfCancellationRequested();

            if (views.TryGetValue(pos, out var view) && view != null)
            {
                views.Remove(pos);
                removed.Add(view);
                tasks.Add(view.DestroyAnim(ct));
            }
        }

        await UniTask.WhenAll(tasks);
        tasks.Clear();

        foreach (var v in removed)
        {
            ct.ThrowIfCancellationRequested();
            v.Release();
        }

        removed.Clear();

        var movedViews = new List<(GridPosition To, CellView View)>(plan.GravityMoves.Count);

        foreach (var move in plan.GravityMoves)
        {
            ct.ThrowIfCancellationRequested();

            if (!views.TryGetValue(move.From, out var view) || view == null)
                continue;

            views.Remove(move.From);

            movedViews.Add((move.To, view));
            tasks.Add(view.MoveTo(GridToUI(move.To), ct));
        }

        await UniTask.WhenAll(tasks);
        tasks.Clear();

        foreach (var moved in movedViews)
        {
            ct.ThrowIfCancellationRequested();

            if (views.TryGetValue(moved.To, out var existing) &&
                existing != null &&
                existing != moved.View)
            {
                existing.Release();
            }

            views[moved.To] = moved.View;
        }

        movedViews.Clear();

        foreach (var spawn in plan.SpawnMoves)
        {
            ct.ThrowIfCancellationRequested();

            var view = await tileFactory.Create(spawn.tileType, ct);
            if (ct.IsCancellationRequested)
            {
                view.Release();
                ct.ThrowIfCancellationRequested();
            }

            view.transform.SetParent(transform, false);
            view.SetPosition(GridToUI(spawn.From));

            if (views.TryGetValue(spawn.To, out var existing) &&
                existing != null &&
                existing != view)
            {
                existing.Release();
            }

            views[spawn.To] = view;

            tasks.Add(view.MoveTo(GridToUI(spawn.To), ct));
        }

        await UniTask.WhenAll(tasks);
    }

 
    public async UniTask AnimateSwap(GridPosition from, GridPosition to, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!views.TryGetValue(from, out var fromView) || fromView == null)
            return;

        if (!views.TryGetValue(to, out var toView) || toView == null)
            return;

        views[from] = toView;
        views[to] = fromView;

        await UniTask.WhenAll(
            fromView.MoveTo(GridToUI(to), ct),
            toView.MoveTo(GridToUI(from), ct)
        );
    }

   
    private Vector2 GridToUI(GridPosition position)
    {
        var stepX = cellSize + spacing.x;
        var stepY = cellSize + spacing.y;

        return new Vector2(
            gridOrigin.x + (position.X * stepX),
            gridOrigin.y - (position.Y * stepY)
        );
    }

    private void ReleaseAllViews()
    {
        foreach (var view in views.Values)
        {
            if (view != null)
                view.Release();
        }

        views.Clear();
    }

    private void OnDestroy()
    {
        ReleaseAllViews();
    }
}
