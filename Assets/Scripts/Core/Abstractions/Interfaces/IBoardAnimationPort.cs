using Cysharp.Threading.Tasks;
using System.Threading;

public interface IBoardAnimationPort
{
    UniTask PlaySwap(GridPosition from, GridPosition to, CancellationToken ct);
    UniTask PlayResolutionStep(ResolutionStepPlan plan, CancellationToken ct);
    UniTask PlayGridRefresh(GridSnapshot grid, CancellationToken ct);
}
