using Cysharp.Threading.Tasks;
using System.Threading;

public interface ILoadCoreSceneUseCase
{
    UniTask Execute(CancellationToken token);
}