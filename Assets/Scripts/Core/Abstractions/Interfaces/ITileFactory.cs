using Cysharp.Threading.Tasks;
using System.Threading;

public interface ITileFactory
{
    UniTask<CellView> Create(TileType tileType, CancellationToken ct);

}
