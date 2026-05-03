using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using VContainer;

public class AnimatedMatchResolutionService : IMatchResolutionService
{
    private bool isResolving;
    private IMatchResolutionEngine matchResolutionEngine;
    private IMatchRewardService matchRewardService;
    private IResolutionStepApplier resolutionStepApplier;
    private IBoardAnimationPort boardAnimationPort;

    [Inject]
    private void Construct( IMatchResolutionEngine matchResolutionEngine,IMatchRewardService matchRewardService, IResolutionStepApplier resolutionStepApplier,
        IBoardAnimationPort boardAnimationPort)
    {
        this.matchResolutionEngine = matchResolutionEngine ?? throw new ArgumentNullException(nameof(matchResolutionEngine));
        this.matchRewardService = matchRewardService ?? throw new ArgumentNullException(nameof(matchRewardService));
        this.resolutionStepApplier = resolutionStepApplier ?? throw new ArgumentNullException(nameof(resolutionStepApplier));
        this.boardAnimationPort = boardAnimationPort ?? throw new ArgumentNullException(nameof(boardAnimationPort));
    }

    public async UniTask Resolve(GridModel grid, CancellationToken ct)
    {
        if (isResolving)
            return;

        isResolving = true;
        try
        {
            while (matchResolutionEngine.TryBuildNextStep(grid, out var step))
            {
                matchRewardService.Award(step.Matches);

                await boardAnimationPort.PlayResolutionStep(step.Plan, ct);
                resolutionStepApplier.Apply(grid, step.Plan);

                await UniTask.Delay(40, cancellationToken: ct);
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate, ct);
            }

            if (matchResolutionEngine.TryReshuffle(grid, out var reshuffledGrid))
                await boardAnimationPort.PlayGridRefresh(reshuffledGrid, ct);
        }
        finally
        {
            isResolving = false;
        }
    }
}
