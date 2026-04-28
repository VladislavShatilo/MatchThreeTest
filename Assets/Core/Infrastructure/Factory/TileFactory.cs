using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.AddressableAssets;
using VContainer;

public class TileFactory : ITileFactory
{
    private TileDatabase database;
    private Dictionary<TileType, AssetReferenceGameObject> map;


    [Inject]
    private void Construct(TileDatabase database)
    {
        this.database = database;
        map = database.Tiles.ToDictionary(x => x.Type, x => x.Prefab);

    }

    public async UniTask<CellView> Create(TileType type)
    {
        if (!map.TryGetValue(type, out var prefab))
            throw new Exception($"No prefab for {type}");

        var obj = await prefab.InstantiateAsync().ToUniTask();
        return obj.GetComponent<CellView>();
    }
}