using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using VContainer;
using VContainer.Unity;

public class CoreEntryPoint : IAsyncStartable, IDisposable
{
    private IGridService gridService;
    private IGridFactory gridFactory;
    private IBoardAnimationPort boardAnimationPort;
    private CancellationTokenSource startRenderCts;

    [Inject]
    private void Construct(IGridService gridService, IGridFactory gridFactory, IBoardAnimationPort boardAnimationPort)
    {
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
        this.gridFactory = gridFactory ?? throw new ArgumentNullException(nameof(gridFactory));
        this.boardAnimationPort = boardAnimationPort ?? throw new ArgumentNullException(nameof(boardAnimationPort));
    }

    public async UniTask StartAsync(CancellationToken cancellation)
    {
        var gridModel = gridFactory.Create(8, 8);
        gridService.Initialize(gridModel);

        var grid = gridService.Grid;
        if (grid == null)
            throw new InvalidOperationException("Grid is not initialized");

        startRenderCts = new CancellationTokenSource();
        await boardAnimationPort.PlayGridRefresh(new GridSnapshot(grid), startRenderCts.Token);
    }

    public void Dispose()
    {
        startRenderCts?.Cancel();
        startRenderCts?.Dispose();
        startRenderCts = null;
    }
}
