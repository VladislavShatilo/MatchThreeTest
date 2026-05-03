using System.Threading;
using Cysharp.Threading.Tasks;

public interface IMatchResolutionService 
{
    UniTask Resolve(GridModel grid, CancellationToken ct);
}


