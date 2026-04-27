using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public interface ILoadingScreenFactory
{
    UniTask<GameObject> Create(CancellationToken token);
}