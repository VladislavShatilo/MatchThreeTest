using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class GridView : MonoBehaviour
{
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector2 spacing = Vector2.zero;
    [SerializeField] private Vector2 gridOrigin = Vector2.zero;


    private IGridService gridService;
    private ITileFactory tileFactory;
    private IEventBus eventBus;
    private IEventAwaiter eventAwaiter;


    private readonly Dictionary<Vector2Int, CellView> views = new();

    [Inject]
    private void Construct(IGridService gridService, ITileFactory tileFactory, IEventBus eventBus, IEventAwaiter eventAwaiter)
    {
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
        this.tileFactory = tileFactory ?? throw new ArgumentNullException(nameof(tileFactory));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.eventAwaiter = eventAwaiter ?? throw new ArgumentNullException(nameof(eventAwaiter));
    }

    private void Awake()
    {
        DisableAutoLayout();
    }

    private void Start()
    {
        Build().Forget();
    }
    public Vector2Int GetGridPosition(Vector3 worldPos)
    {
        var local = transform.InverseTransformPoint(worldPos);
        var stepX = cellSize + spacing.x;
        var stepY = cellSize + spacing.y;
        var x = Mathf.RoundToInt((local.x - gridOrigin.x) / stepX);
        var y = Mathf.RoundToInt((gridOrigin.y - local.y) / stepY);
        return new Vector2Int(x, y);
    }

    public bool TryGetGridPosition(CellView view, out Vector2Int position)
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

    public CellView GetCellView(Vector2Int pos)
    {
        views.TryGetValue(pos, out var view);
        return view;
    }
    private async UniTask Build()
    {
        while (gridService.Grid == null)
            await UniTask.Yield();

        foreach (var view in views.Values)
        {
            if (view != null)
                Destroy(view.gameObject);
        }
        views.Clear();

        var grid = gridService.Grid;

        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                var cell = grid.Get(x, y);
                if (cell == null)
                    continue;

                var view = await tileFactory.Create(cell.Type);
                view.transform.SetParent(transform, false);

                views[new Vector2Int(x, y)] = view;

                view.SetPosition(GridToUI(x, y));
            }
        }
    }


   
    public async UniTask PlayPlan(ResolutionStepPlan plan)
    {
        var ct = this.GetCancellationTokenOnDestroy();
        var tasks = new List<UniTask>();

        foreach (var pos in plan.RemovedCells)
        {
            if (views.TryGetValue(pos, out var view) && view != null)
            {
                views.Remove(pos);
                tasks.Add(view.DestroyAnim());
            }
        }

        await UniTask.WhenAll(tasks);
        tasks.Clear();
        var removeAppliedTask = eventAwaiter.WaitAsync<GridStepAppliedEvent>(e => e.Phase == GridAnimationPhase.Remove, ct);
        eventBus.Publish(new GridAnimationCompletedEvent(GridAnimationPhase.Remove));
        await removeAppliedTask;

        var movedViews = new List<(Vector2Int To, CellView View)>(plan.GravityMoves.Count);
        foreach (var move in plan.GravityMoves)
        {
            if (!views.TryGetValue(move.From, out var view) || view == null)
                continue;

            views.Remove(move.From);
            movedViews.Add((move.To, view));
            tasks.Add(view.MoveTo(GridToUI(move.To.x, move.To.y)));
        }

        foreach (var moved in movedViews)
        {
            if (views.TryGetValue(moved.To, out var existing) && existing != null && existing != moved.View)
            {
                Destroy(existing.gameObject);
            }

            views[moved.To] = moved.View;
        }

        await UniTask.WhenAll(tasks);
        tasks.Clear();
        var gravityAppliedTask = eventAwaiter.WaitAsync<GridStepAppliedEvent>(e => e.Phase == GridAnimationPhase.Gravity, ct);
        eventBus.Publish(new GridAnimationCompletedEvent(GridAnimationPhase.Gravity));
        await gravityAppliedTask;

        foreach (var spawn in plan.SpawnMoves)
        {
            var view = await tileFactory.Create(spawn.tileType);
            if (view == null)
                continue;

            view.transform.SetParent(transform, false);
            view.SetPosition(GridToUI(spawn.From.x, spawn.From.y));

            if (views.TryGetValue(spawn.To, out var existing) && existing != null && existing != view)
            {
                Destroy(existing.gameObject);
            }

            views[spawn.To] = view;

            tasks.Add(view.MoveTo(GridToUI(spawn.To.x, spawn.To.y)));
        }

        await UniTask.WhenAll(tasks);
        var refillAppliedTask = eventAwaiter.WaitAsync<GridStepAppliedEvent>(e => e.Phase == GridAnimationPhase.Refill, ct);
        eventBus.Publish(new GridAnimationCompletedEvent(GridAnimationPhase.Refill));
        await refillAppliedTask;
        eventBus.Publish(new GridAnimationCompletedEvent(GridAnimationPhase.StepCompleted));
    }

    public async UniTask PlaySwap(Vector2Int from, Vector2Int to)
    {
        if (!views.TryGetValue(from, out var fromView) || fromView == null)
            return;
        if (!views.TryGetValue(to, out var toView) || toView == null)
            return;

        views[from] = toView;
        views[to] = fromView;

        await UniTask.WhenAll(
            fromView.MoveTo(GridToUI(to.x, to.y)),
            toView.MoveTo(GridToUI(from.x, from.y)));
    }

    private Vector2 GridToUI(int x, int y)
    {
        var stepX = cellSize + spacing.x;
        var stepY = cellSize + spacing.y;
        return new Vector2(
            gridOrigin.x + (x * stepX),
            gridOrigin.y - (y * stepY));
    }

    private void DisableAutoLayout()
    {
        var layoutGroup = GetComponent<GridLayoutGroup>();
        if (layoutGroup != null)
            layoutGroup.enabled = false;

        var genericLayout = GetComponent<LayoutGroup>();
        if (genericLayout != null)
            genericLayout.enabled = false;

        var contentSizeFitter = GetComponent<ContentSizeFitter>();
        if (contentSizeFitter != null)
            contentSizeFitter.enabled = false;
    }

}