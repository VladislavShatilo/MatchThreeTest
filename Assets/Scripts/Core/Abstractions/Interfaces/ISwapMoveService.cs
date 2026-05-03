using Cysharp.Threading.Tasks;
using System.Threading;

public interface ISwapMoveService
{
    UniTask<bool> TryExecute(SwapRequest request, CancellationToken token);
}
