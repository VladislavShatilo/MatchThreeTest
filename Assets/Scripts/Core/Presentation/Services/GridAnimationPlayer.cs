using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using VContainer;

public class GridAnimationPlayer : IBoardAnimationPort
{
    private GridView gridView;

    [Inject]
    private void Construct(GridView gridView)
    {
        this.gridView = gridView ?? throw new ArgumentNullException(nameof(gridView));
    }

    public UniTask PlaySwap(GridPosition from, GridPosition to, CancellationToken ct)
    {
        return gridView.AnimateSwap(from, to, ct);
    }

    public UniTask PlayResolutionStep(ResolutionStepPlan plan, CancellationToken ct)
    {
        return gridView.AnimateResolutionStep(plan, ct);
    }

    public UniTask PlayGridRefresh(GridSnapshot grid, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();
        return gridView.Render(grid, ct);
    }
}
