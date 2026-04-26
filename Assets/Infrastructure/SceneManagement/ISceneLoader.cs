using Cysharp.Threading.Tasks;
using System.Threading;

public interface ISceneLoader
{
    UniTask LoadScene(string sceneKey, CancellationToken token);
}