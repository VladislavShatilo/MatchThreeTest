using Cysharp.Threading.Tasks;
using System.Threading;

public interface ILoadingScreenService
{
    UniTask Init(CancellationToken token);

    UniTask Show(CancellationToken token);
    UniTask Hide( CancellationToken token);
}