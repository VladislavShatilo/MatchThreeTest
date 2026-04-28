using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public interface IMatchResolutionService 
{
    UniTask Resolve(GridModel grid, CancellationToken ct);
}
