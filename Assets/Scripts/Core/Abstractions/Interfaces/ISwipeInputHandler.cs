using Cysharp.Threading.Tasks;
using System.Threading;

public interface ISwipeInputHandler
{
    UniTask HandleSwipe(SwipeIntent intent, CancellationToken token);
}
