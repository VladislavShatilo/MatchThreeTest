using Cysharp.Threading.Tasks;
using System.Threading;

public interface ISwapCommand
{
    UniTask Execute(CancellationToken token);
}


