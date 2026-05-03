using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;

public class TileFactory : ITileFactory
{
    private TileDatabase database;
    private Dictionary<TileType, AssetReferenceGameObject> map;

    [Inject]
    private void Construct(TileDatabase database)
    {
        this.database = database ?? throw new ArgumentNullException(nameof(database));

        map = database.Tiles.ToDictionary(x => x.Type, x => x.Prefab);
    }

    public async UniTask<CellView> Create(TileType type, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!map.TryGetValue(type, out var prefab))
            throw new Exception($"No prefab for {type}");

        AsyncOperationHandle<GameObject> handle = prefab.InstantiateAsync();

        GameObject obj;
        try
        {
            obj = await handle.ToUniTask(cancellationToken: ct);
        }
        catch
        {
            if (handle.IsValid())
                Addressables.Release(handle);

            throw;
        }

        if (ct.IsCancellationRequested)
        {
            Addressables.ReleaseInstance(obj);
            ct.ThrowIfCancellationRequested();
        }

        var view = obj.GetComponent<CellView>();
        if (view == null)
        {
            Addressables.ReleaseInstance(obj);
            throw new Exception($"Prefab for {type} does not contain {nameof(CellView)}");
        }

        view.MarkAsAddressableInstance();
        return view;
    }
}
