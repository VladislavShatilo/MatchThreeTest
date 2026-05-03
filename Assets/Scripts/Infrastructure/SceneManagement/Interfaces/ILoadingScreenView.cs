using Cysharp.Threading.Tasks;
using System.Threading;

public interface ILoadingScreenView
{
    UniTask Show(CancellationToken token);
    UniTask Hide(CancellationToken token);
}