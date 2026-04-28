using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;
public class SwapAnimator : MonoBehaviour
{
    [SerializeField] private float animationTime = 0.2f;
    private Dictionary<Vector2Int, CellView> views;

    public void Initialize(Dictionary<Vector2Int, CellView> views)
    {
        this.views = views;
    }

    public async UniTask Play(Vector2Int from, Vector2Int to, CancellationToken ct)
    {
        var a = views[from];
        var b = views[to];

        var posA = a.transform.position;
        var posB = b.transform.position;

        await UniTask.WhenAll(
            a.transform.DOMove(posB, animationTime).AsyncWaitForCompletion().AsUniTask(),
            b.transform.DOMove(posA, animationTime).AsyncWaitForCompletion().AsUniTask()
        );
    }
}
