using Cysharp.Threading.Tasks;
using System.Threading;

public interface ISwapCommandHandler
{
    UniTask Execute(SwapCommand command, CancellationToken token);
}