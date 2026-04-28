using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public interface IEventAwaiter 
{
    UniTask WaitAsync<T>(CancellationToken ct);
    UniTask<T> WaitAsync<T>(Func<T, bool> predicate, CancellationToken ct);
}
